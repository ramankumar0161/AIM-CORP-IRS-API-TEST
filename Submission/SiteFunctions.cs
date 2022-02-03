using IrsSubmission.Models;
using MeF.Client;
using MeF.Client.EfileAttachments;
using MeF.Client.Services.InputComposition;
using MeF.Client.Services.MSIServices;
using MeF.Client.Services.TransmitterServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using static IrsSubmission.Controllers.ValuesController;

namespace IrsSubmission.Submission
{
    public class SiteFunctions
    {
        private static ServiceContext context = new ServiceContext();

        public SubmissionResponse SendSubmission(submissionModel submission)
        {
            SubmissionResponse oResonse = new SubmissionResponse();
            try
            {
                if (submission.environment == "p" || submission.environment == "P")
                {
                    context = new ServiceContext(new ClientInfo(submission.etin, submission.appSysId,WCFClient.TestCdType.P));
                }
                else
                {
                    context = new ServiceContext(new ClientInfo(submission.etin, submission.appSysId, WCFClient.TestCdType.T));
                }
                
                if (context != null)
                {
                    LoginClient client = new LoginClient();
                    LoginResult Loginresult = client.Invoke(context);
                    // Create object of SubmissionBuilder over here
                    var builder = new SubmissionBuilder();
                    // We need input SubmissionID as input for this
                    string SubmissionID = submission.submissionId;
                    // Use the manifest file name uploaded earlier and create "manifest" for submission
                    var manifest = new SubmissionManifest("manifest.xml", Encoding.UTF8.GetString(Convert.FromBase64String(submission.Manifest)));
                    // Use the XML file name uploaded earlier and create "XML" for submission
                    var xml = new SubmissionXml("xml.xml", Encoding.UTF8.GetString(Convert.FromBase64String(submission.XML)));
                    // Use the PDF file name uploaded earlier and create "BinaryAttchments" for submission. You can also use loop here to add multiple pdfs is you want to add multiple.
                    var attachments = new List<BinaryAttachment>();
                    byte[] pdfbyte = Convert.FromBase64String(submission.PDF);
                    var b = new BinaryAttachment("manifest.pdf", pdfbyte);
                    //attachments.Add(b);
                    // Let's invoke the Create Submission Archive methods using "SubmissionID", "manifest", "XML", and path: where you want to store the Archive generated from this.
                    // Replace the path "D:\Aim Corporation\Desktop Application\SubmissionArchive" with path where you want to store the Archives
                    var arch = builder.CreateIRSSubmissionArchive(SubmissionID, manifest, xml, attachments);
                    // Create Postmarked Submission using Archive just created and DateTime Stamp
                    PostmarkedSubmissionArchive archive = builder.CreatePostmarkedSubmissionArchive(arch, DateTime.Now);
                    // Add the PostMarked Archives to list
                    List<PostmarkedSubmissionArchive> archives = new List<PostmarkedSubmissionArchive>();
                    archives.Add(archive);
                    // Create object for SendSubmission using path: where you want to store the submissions
                    // Relpace the path : "D:\Aim Corporation\Desktop Application\Container" with path where you want to keep the submissions results
                    SendSubmissionsClient client1 = new SendSubmissionsClient();
                    // Create Container which we will send to IRS as Submission using path and Archive
                    // Replace the path "D:\Aim Corporation\Desktop Application\Container" with path where you actully want to store the Container files on system
                    SubmissionContainer container = builder.CreateSubmissionContainer(archives);
                    // Here we send the submissions to IRS using Globle ServiceContext and container just created
                    SendSubmissionsResult result = client1.Invoke(context, container);
                    // Here we get the output data for submissions
                    SubmissionReceiptList receipts = result.GetSubmissionReceiptList();
                    SubmissionReceiptGrp receiptGrp = receipts.FindBySubmissionId(SubmissionID);
                    oResonse.MessageID = receiptGrp.SubmissionId;
                    oResonse.Relatesto = receiptGrp.SubmissionReceivedTs.ToString();
                    oResonse.Statustxt = "IRS submission done successfully.";
                    LogoutClient logoutClient = new LogoutClient();
                    LogoutResult logoutResult = logoutClient.Invoke(context);
                    return oResonse;
                }
                else
                {
                    oResonse.MessageID = "";
                    oResonse.Relatesto = "";
                    oResonse.Statustxt = "";
                    return oResonse;
                }
            }
            catch (Exception ex)
            {
                oResonse.MessageID = "";
                oResonse.Relatesto = "";
                oResonse.Statustxt = ex.Message;
                return oResonse;
            }
        }
        public AckResponse GetAck(PostAck Ack)
        {
            string msg = "";
            AckResponse oResonse = new AckResponse();
            try
            {
                if (Ack.environment == "p" || Ack.environment == "P")
                {
                    context = new ServiceContext(new ClientInfo(Ack.etin, Ack.appSysId, WCFClient.TestCdType.P));
                }
                else
                {
                    context = new ServiceContext(new ClientInfo(Ack.etin, Ack.appSysId, WCFClient.TestCdType.T));
                }
                
                if (context != null)
                {
                    LoginClient client = new LoginClient();
                    msg = "login start";
                    LoginResult Loginresult = client.Invoke(context);
                    msg = "login success";
                    // Create object of SubmissionBuilder over here
                   // GetAckClient ackClient = new GetAckClient(@"D:\Aim Corporation\Desktop Application\Acknowledgement");
                    GetAckClient ackClient = new GetAckClient();
                    msg = "ack client start";
                    GetAckResult result = ackClient.Invoke(context, Ack.submissionId);
                    msg = "ack success";
                    AcknowledgementList acks = result.GetAcknowledgementList();
                    var attchmentFilePath = result.AttachmentFilePath;
                    oResonse.MessageID = result.MessageID;
                    oResonse.content = result.unzippedcontent;
                    oResonse.Relatesto = result.RelatesTo;
                    oResonse.Statustxt = "OK";
                    LogoutClient logoutClient = new LogoutClient();
                    LogoutResult logoutResult = logoutClient.Invoke(context);
                    return oResonse;
                }
                else
                {
                    oResonse.MessageID = "";
                    oResonse.Relatesto = "";
                    oResonse.Statustxt = "";
                    return oResonse;
                }
            }
            catch (Exception ex)
            {
                oResonse.MessageID = "";
                oResonse.Relatesto = msg;
                oResonse.Statustxt = ex.ToString();
                return oResonse;
            }
        }
    }
}