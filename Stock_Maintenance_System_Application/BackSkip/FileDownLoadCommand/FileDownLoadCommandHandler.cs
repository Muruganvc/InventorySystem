using InventorySystem_Application.Common;
using InventorySystem_Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_Application.BackSkip.FileDownLoadCommand;
internal sealed class FileDownLoadCommandHandler : IRequestHandler<FileDownLoadCommand, IResult<bool>>
{
    private readonly IRepository<InventorySystem_Domain.User> _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    public FileDownLoadCommandHandler(IRepository<InventorySystem_Domain.User> companyRepository, IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult<bool>> Handle(FileDownLoadCommand request, CancellationToken cancellationToken)
    {
        var usersWithImages = await _companyRepository
            .Table
            .Where(u => u.ProfileImage != null && u.ProfileImage.Length > 0)
            .ToListAsync(cancellationToken);

        foreach (var user in usersWithImages)
        {
            var extension = GetFileExtension(user.ProfileImage!);
            var filePath = Path.Combine($"{request.DownloadPath}", $"{user.UserId}{extension}");
            await File.WriteAllBytesAsync(filePath, user.ProfileImage!, cancellationToken);
            user.ProfileImage = null;
        }
        var saveResult = await _unitOfWork.SaveAsync() > 0;
        return Result<bool>.Success(saveResult);
    }

    static string GetFileExtension(byte[] bytes)
    {
        if (bytes.Length >= 4)
        {
            // Check PDF
            if (bytes[0] == 0x25 && bytes[1] == 0x50 && bytes[2] == 0x44 && bytes[3] == 0x46)
                return ".pdf";

            // Check PNG
            if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
                return ".png";

            // Check JPG
            if (bytes[0] == 0xFF && bytes[1] == 0xD8)
                return ".jpg";

            // Check DOCX / XLSX / ZIP
            if (bytes[0] == 0x50 && bytes[1] == 0x4B)
                return ".docx"; // or .xlsx or .zip depending on content

            // Check EXE
            if (bytes[0] == 0x4D && bytes[1] == 0x5A)
                return ".exe";
        }

        return ".bin"; // Unknown binary
    }
}
