using Microsoft.AspNetCore.Http;

namespace CoursesManagmentMicroservices.Core.ServiceContract
{
    public interface IFileServices
    {
        /// <summary>
        /// Creates a new file and saves it to the specified location.
        /// </summary>
        /// <param name="file">The file to be created.</param>
        /// <returns>The name of the newly created file.</returns>
        Task<string> CreateFileAsync(IFormFile file);

        /// <summary>
        /// Deletes the file with the specified image URL.
        /// </summary>
        /// <param name="imageUrl">The URL of the file to be deleted.</param>
        Task DeleteFileAsync(string? imageUrl);

        /// <summary>
        /// Updates the file with a new file and deletes the file with the specified current file name.
        /// </summary>
        /// <param name="newFile">The new file to be updated.</param>
        /// <param name="currentFileName">The name of the current file to be deleted.</param>
        /// <returns>The name of the newly created file.</returns>
        Task<string> UpdateFileAsync(IFormFile newFile, string? currentFileName);
    }
}
