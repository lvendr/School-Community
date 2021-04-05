using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab4.Data;
using Lab4.Models.ViewModels;
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
        private readonly SchoolCommunityContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string containerName = "image";

        public AdvertisementsController(SchoolCommunityContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public async Task<IActionResult> Index(string ID)
        {
            var viewModel = new AdsViewModel();
            viewModel.Community = _context.Communities.Find(ID);
            viewModel.Advertisements = await _context.Advertisements
                                    .Where(i => i.CommunityId == ID)
                                    .AsNoTracking()
                                    .ToListAsync();
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create(string ID)
        {
            var viewModel = new AdsViewModel();
            viewModel.Community = _context.Communities.Find(ID);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile ads, string communityID, string communityTitle)
        {
            if (ads == null)
                return RedirectToAction("Index", new { ID = communityID });

            BlobContainerClient containerClient;
            // Create the container and return a container client object
            try
            {
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName);
                // Give access to public
                containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            catch (RequestFailedException)
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }

            try
            {
                // create the blob to hold the data
                var blockBlob = containerClient.GetBlobClient(ads.FileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                using (var memoryStream = new MemoryStream())
                {
                    // copy the file data into memory
                    await ads.CopyToAsync(memoryStream);

                    // navigate back to the beginning of the memory stream
                    memoryStream.Position = 0;

                    // send the file to the cloud
                    await blockBlob.UploadAsync(memoryStream);
                    memoryStream.Close();
                }

                // add the photo to the database if it uploaded successfully
                var image = new Advertisements();
                image.Url = blockBlob.Uri.AbsoluteUri;
                image.FileName = ads.FileName;
                image.CommunityId = communityID;
                image.CommunityTitle = communityTitle;


                _context.Advertisements.Add(image);
                _context.SaveChanges();
            }
            catch (RequestFailedException)
            {
                View("Error");
            }

            return RedirectToAction("Index", new { ID = communityID });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Advertisements
                .FirstOrDefaultAsync(m => m.ID == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ID, string communityID)
        {
            var image = await _context.Advertisements.FindAsync(ID);
            BlobContainerClient containerClient;

                // Get the container and return a container client object
                try
                {
                    containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                }
                catch (RequestFailedException)
                {
                    return View("Error");
                }

            try
            {
                // Get the blob that holds the data
                var blockBlob = containerClient.GetBlobClient(image.FileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                _context.Advertisements.Remove(image);
                await _context.SaveChangesAsync();

            }
            catch (RequestFailedException)
            {
                return View("Error");
            }

            return RedirectToAction("Index", new { ID = communityID });
        }
    }
}
