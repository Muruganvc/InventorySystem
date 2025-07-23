using InventorySystem_Application.Common;
using InventorySystem_Domain;
using InventorySystem_Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_Application.BackSkip.RestoreFileDataCommand;
internal sealed class RestoreFileDataCommandHandler : IRequestHandler<RestoreFileDataCommand, IResult<bool>>
{
    private readonly IRepository<InventorySystem_Domain.User> _userRepository;
    private readonly IRepository<FileDataBackup> _fileDataBackRepository;
    private readonly IRepository<InventorySystem_Domain.InventoryCompanyInfo> _invCompanyInfoRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RestoreFileDataCommandHandler(IRepository<InventorySystem_Domain.User> userRepository, IUnitOfWork unitOfWork, IRepository<FileDataBackup> fileDataBackRepository,
      IRepository<InventorySystem_Domain.InventoryCompanyInfo> invCompanyInfoRepository)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _fileDataBackRepository = fileDataBackRepository;
        _invCompanyInfoRepository = invCompanyInfoRepository;
    }

    public async Task<IResult<bool>> Handle(RestoreFileDataCommand request, CancellationToken cancellationToken)
    {
        var userBackups = await _fileDataBackRepository.Table.Where(a => a.TableName == "Users").ToListAsync(cancellationToken);

        await RestoreFileDataAsync(userBackups,
            async id => await _userRepository.GetByAsync(u => u.UserId == id),
            (user, data) => user.ProfileImage = data
        );

        var companyBackups = await _fileDataBackRepository.Table.Where(a => a.TableName == "InventoryCompanyInfo").ToListAsync(cancellationToken);
        await RestoreFileDataAsync(
            companyBackups,
            async id => await _invCompanyInfoRepository.GetByAsync(c => c.InventoryCompanyInfoId == id),
            (company, data) => company.QcCode = data
        );

        var saveResult = await _unitOfWork.SaveAsync() > 0;
        return Result<bool>.Success(saveResult);
    }

    private async Task RestoreFileDataAsync<T>(List<FileDataBackup> backups, Func<int, Task<T?>> getEntityByIdAsync,
    Action<T, byte[]> setFileData) where T : class
    {
        foreach (var backup in backups)
        {
            var entity = await getEntityByIdAsync(backup.UniqueId);
            if (entity != null)
            {
                setFileData(entity, backup.FileData);
            }
        }
    }
}