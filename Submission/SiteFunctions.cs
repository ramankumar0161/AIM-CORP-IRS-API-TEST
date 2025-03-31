using IrsSubmission.Models;
using MeF.Client;
using MeF.Client.EfileAttachments;
using MeF.Client.Services.InputComposition;
using MeF.Client.Services.MSIServices;
using MeF.Client.Services.TransmitterServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

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
                context = new ServiceContext(new ClientInfo(submission.etin, submission.appSysId, WCFClient.TestCdType.T));

                if (context != null)
                {
                    LoginClient client = new LoginClient();
                    LoginResult Loginresult = client.Invoke(context);
                    var builder = new SubmissionBuilder();
                    string SubmissionID = submission.submissionId;
                    var manifest = new SubmissionManifest("manifest.xml", Encoding.UTF8.GetString(Convert.FromBase64String(submission.Manifest)));
                    var xml = new SubmissionXml("xml.xml", Encoding.UTF8.GetString(Convert.FromBase64String(submission.XML)));
                    var attachments = new List<BinaryAttachment>();
                    byte[] pdfbyte = Convert.FromBase64String(submission.PDF);
                    var b = new BinaryAttachment("manifest.pdf", pdfbyte);
                    var arch = builder.CreateIRSSubmissionArchive(SubmissionID, manifest, xml, attachments);
                    PostmarkedSubmissionArchive archive = builder.CreatePostmarkedSubmissionArchive(arch, DateTime.Now);
                    List<PostmarkedSubmissionArchive> archives = new List<PostmarkedSubmissionArchive>();
                    archives.Add(archive);
                    SendSubmissionsClient client1 = new SendSubmissionsClient();
                    SubmissionContainer container = builder.CreateSubmissionContainer(archives);
                    SendSubmissionsResult result = client1.Invoke(context, container);
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
                oResonse.Statustxt = ex.InnerException.Message.ToString();
                LogoutClient logoutClient = new LogoutClient();
                LogoutResult logoutResult = logoutClient.Invoke(context);
                return oResonse;
            }
        }
        public AckResponse GetAck(PostAck Ack)
        {
            string msg = "";
            AckResponse oResonse = new AckResponse();
            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = @"Files\Certificate\AimCertificateForIRS20230615\AimCertificateForIRS20230615.pfx";
                string certFilePath = Path.Combine(currentDirectory, filePath);
                string certPassword = "Bebomat01@@";
                X509Certificate2 certificate = new X509Certificate2(certFilePath, certPassword);

                context = new ServiceContext(new ClientInfo(Ack.etin, Ack.appSysId, WCFClient.TestCdType.T));
                

                if (context != null)
                {
                    LoginClient client = new LoginClient();
                   
                    msg = "login start";
                    LoginResult Loginresult = client.Invoke(context);
                    msg = "login success";
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
                oResonse.Statustxt = ex.InnerException.Message.ToString();
                LogoutClient logoutClient = new LogoutClient();
                LogoutResult logoutResult = logoutClient.Invoke(context);
                return oResonse;
            }
        }
    }
}