using InventorySystem_Application.Common;
using InventorySystem_Domain;
using InventorySystem_Domain.Common;
using MediatR;

namespace InventorySystem_Application.BackSkip.FileUpLoadCommand;

internal sealed class FileUpLoadCommandHandler : IRequestHandler<FileUpLoadCommand, IResult<bool>>
{
    private readonly IRepository<InventorySystem_Domain.User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public FileUpLoadCommandHandler(IRepository<InventorySystem_Domain.User> userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<IResult<bool>> Handle(FileUpLoadCommand request, CancellationToken cancellationToken)
    {
        var files = Directory.GetFiles(request.UploadPath);
        foreach (var filePath in files)
        {
            byte[] fileBytes = await File.ReadAllBytesAsync(filePath, cancellationToken);
            string fileName = Path.GetFileName(filePath);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
            var user = await _userRepository.GetByAsync(u => u.UserId == Convert.ToInt32(fileNameWithoutExt));
            if (user != null)
            {
                user.ProfileImage = fileBytes;
                bool isSuccess = false;
                await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    isSuccess = await _unitOfWork.SaveAsync() > 0;
                }, cancellationToken);
            }
        }
        return Result<bool>.Success(true);
    }
}
