using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ZXing;

namespace QRCodeAPI.Controllers
{
    public class SAPController : ApiController
    {
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("GenerateQRCode")]
        public HttpResponseMessage GenerateQRCode(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Text parameter is required.");
            }

            using (var memoryStream = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.M); // Adjust error correction as needed
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(5); // Adjust size as desired

                // **Fix 1: Save to memory stream instead of file system**
                qrCodeImage.Save(memoryStream, ImageFormat.Jpeg);

                // Get bytes from the memory stream
                byte[] imageBytes = memoryStream.ToArray();

                // Return the image directly
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(imageBytes);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                return result;
            }
        }

    }
}
//https://localhost:44381/api/SAP/GenerateQRCode?text=Hello%20Utkarsh