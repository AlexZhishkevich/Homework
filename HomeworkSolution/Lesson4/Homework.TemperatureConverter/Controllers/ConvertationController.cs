using Homework.TemperatureConverter.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Homework.TemperatureConverter.Controllers
{
    public class ConvertationController : Controller
    {
        private readonly IValidation<float?> _valueValidator;
        private static IActionResult _result;

        private readonly IDictionary<byte, Action<float>> _exportFunctions = new Dictionary<byte, Action<float>>()
        {
            {0, ExportAsZipFile},
            {1, ExportAsTxtFile},
            {2, ExportAsByteStream}
        };

        private const string ResponseTxtFileName = "FahrenheitValue.txt";
        private const string ResponseZipFileName = "FahrenheitValueArchive.zip";

        public ConvertationController(IValidation<float?> validator)
        {
            _valueValidator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        [Route("Convertation/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Convertation/Convert")]
        [Route("Convertation/Convert/{inputTemperature}")]
        public IActionResult Convert(float? inputTemperature)
        {
            var validationResult = _valueValidator.ValidateValue(inputTemperature);

            if (validationResult.IsValid)
            {
                var fahrenheit = (inputTemperature * (9 / 5)) + 32;
                var responseString = $"Result is {fahrenheit} °F";
                ViewData["OutputMessage"] = responseString;
                ViewData["ResultValue"] = fahrenheit;
            }
            else
                return BadRequest($"Bad request \n {validationResult.ErrorMessage}");

            return View();
        }

        [Route("Convertation/ExportResult")]
        [Route("Convertation/ExportResult/{selectedIndex}&{fahrenheitValue}")]
        public IActionResult ExportResult(byte selectedIndex, float fahrenheitValue)
        {
            _exportFunctions[selectedIndex].Invoke(fahrenheitValue);

            return _result;
        }

        private static void ExportAsByteStream(float value)
        {
            var responseByteStream = new MemoryStream(Encoding.UTF8.GetBytes(value.ToString(CultureInfo.InvariantCulture)));
            _result = new FileStreamResult(responseByteStream, "application/octet-stream");
        }

        private static void ExportAsTxtFile(float value)
        {
            _result = new FileContentResult(Encoding.UTF8.GetBytes(value.ToString(CultureInfo.InvariantCulture)), "text/plain")
            {
                FileDownloadName = ResponseTxtFileName
            };
        }

        private static void ExportAsZipFile(float value)
        {
            (string FileName, byte[] Content) fileTuple = (ResponseTxtFileName, Encoding.UTF8.GetBytes(value.ToString(CultureInfo.InvariantCulture)));

            _result = new FileContentResult(CreateZipArchive(fileTuple), "application/zip")
            {
                FileDownloadName = ResponseZipFileName
            };
        }

        private static byte[] CreateZipArchive((string FileName, byte[] Content) fileTuple)
        {
            using var archiveStream = new MemoryStream();
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
            {
                var zipArchiveEntry = archive.CreateEntry(fileTuple.FileName, CompressionLevel.Fastest);
                using var zipStream = zipArchiveEntry.Open();
                zipStream.Write(fileTuple.Content!, 0, fileTuple.Content.Length);
            }

            var archiveFile = archiveStream.ToArray();

            return archiveFile;
        }
    }
}
