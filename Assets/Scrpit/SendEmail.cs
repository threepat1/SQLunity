using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using UnityEngine.UI;

public class SendEmail : MonoBehaviour {
    private static System.Random random = new System.Random();

    public string code;
    public InputField email;
    public string username;

    // Use this for initialization
    public static string RandomString(int length)
    {
        const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
    public void Email()
    {
        code = RandomString(8);

        MailMessage mail = new MailMessage();
        MailAddress ourEmail = new MailAddress("sqlunityclasssydney@gmail.com");
        //Sent to
        mail.To.Add(email.text);
        //Sent from
        mail.From = ourEmail;
        //Topic
        mail.Subject = "SQueaL Games User Reset";
        //Message
        mail.Body = "Hello" + username + "\n Reset your Account using this code: " + code;

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 25;

        smtpServer.Credentials = new System.Net.NetworkCredential
            ("sqlunityclasssydney@gmail.com", "sqlpassword")
            as ICredentialsByHost;

        smtpServer.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback
            = delegate (object s, X509Certificate cert, X509Chain chain
            , SslPolicyErrors policyErrors)
            { return true; };

        smtpServer.Send(mail);
        Debug.Log("Success");
    }

}
