using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab4.Data;
using Lab4.Models;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Azure;
using System.IO;

namespace Lab4.Controllers
{
    public class AdvertisementsController : Controller
    {
        /*private readonly AdsViewModel _context;*/
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string containerName = "image";

        /*public AdvertisementsController(AdsViewModel context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }*/

        /*public async Task<IActionResult> Index()
        {
            return View(await _context.Advertisements.ToListAsync());
        }*/

        /*[HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile answerImage, string Question)
        {
            if (answerImage == null)
                return RedirectToAction("Index");

            BlobContainerClient containerClient;
            if (Question.Equals("Computer"))
            {
                // Create the container and return a container client object
                try
                {
                    containerClient = await _blobServiceClient.CreateBlobContainerAsync(computerContainerName);
                    // Give access to public
                    containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
                }
                catch (RequestFailedException)
                {
                    containerClient = _blobServiceClient.GetBlobContainerClient(computerContainerName);
                }
            }
            else
            {
                {
                    try
                    {
                        containerClient = await _blobServiceClient.CreateBlobContainerAsync(earthContainerName);
                        // Give access to public
                        containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
                    }
                    catch (RequestFailedException)
                    {
                        containerClient = _blobServiceClient.GetBlobContainerClient(earthContainerName);
                    }
                }
            }

            try
            {
                // create the blob to hold the data
                var blockBlob = containerClient.GetBlobClient(answerImage.FileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                using (var memoryStream = new MemoryStream())
                {
                    // copy the file data into memory
                    await answerImage.CopyToAsync(memoryStream);

                    // navigate back to the beginning of the memory stream
                    memoryStream.Position = 0;

                    // send the file to the cloud
                    await blockBlob.UploadAsync(memoryStream);
                    memoryStream.Close();
                }

                // add the photo to the database if it uploaded successfully
                var image = new AnswerImage();
                image.Url = blockBlob.Uri.AbsoluteUri;
                image.FileName = answerImage.FileName;
                if (Question.Equals("Earth"))
                    image.Question = (Question)1;

                _context.AnswerImages.Add(image);
                _context.SaveChanges();
            }
            catch (RequestFailedException)
            {
                View("Error");
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.AnswerImages
                .FirstOrDefaultAsync(m => m.AnswerImageId == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.AnswerImages.FindAsync(id);
            BlobContainerClient containerClient;

            if (image.Question.Equals("Computer"))
            {
                // Get the container and return a container client object
                try
                {
                    containerClient = _blobServiceClient.GetBlobContainerClient(computerContainerName);
                }
                catch (RequestFailedException)
                {
                    return View("Error");
                }
            }
            else
            {
                // Get the container and return a container client object
                try
                {
                    containerClient = _blobServiceClient.GetBlobContainerClient(earthContainerName);
                }
                catch (RequestFailedException)
                {
                    return View("Error");
                }
            }

            try
            {
                // Get the blob that holds the data
                var blockBlob = containerClient.GetBlobClient(image.FileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                _context.AnswerImages.Remove(image);
                await _context.SaveChangesAsync();

            }
            catch (RequestFailedException)
            {
                return View("Error");
            }

            return RedirectToAction("Index");
        }
    }*/
    }
}
