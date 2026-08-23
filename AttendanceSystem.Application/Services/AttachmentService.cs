using AttendanceSystem.Domain;
using Microsoft.AspNetCore.Http;

namespace AttendanceSystem.Application;

public class AttachmentService(
    IUnitOfWork unitOfWork,
    IFileStorageService fileStorageService) : IAttachmentService
{
    readonly IFileStorageService _fileStorageService = fileStorageService;
    readonly IUnitOfWork _unitOfWork = unitOfWork;
    private static readonly string[] AllowedExtensions =
        [".pdf", ".jpg", ".jpeg", ".png"];
    private const long MaxFileSizeBytes = 5 * 1024 * 1024;

    public async Task<List<AttachmentDto>> GetByRequestIdAsync(Guid requestId)
    {
        bool requestExists = await _unitOfWork.AttendanceRequestRepository.IsExistAsync(requestId);
        if (!requestExists) throw new NotFoundException("Attendance request not found");

        IReadOnlyList<AttendanceAttachment> attachments = await _unitOfWork
            .AttachmentRepository.GetByRequestIdAsync(requestId);

        return [.. attachments.Select(x => x.ToDto())];
    }

    public async Task AddAsync(Guid requestId, IFormFile file)
    {
        ValidateFile(file);

        bool requestExists = await _unitOfWork.AttendanceRequestRepository
            .IsExistAsync(requestId);

        if (!requestExists)
            throw new NotFoundException("Attendance request not found");

        string filePath = await _fileStorageService.SaveAsync(file, requestId.ToString());

        AttendanceAttachment attachment = AttendanceAttachment.Create(
            requestId,
            file.FileName,
            filePath,
            file.Length);

        await _unitOfWork.AttachmentRepository.AddAsync(attachment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<(Stream stream, string fileName)> GetFileAsync(Guid attachmentId)
    {
        AttendanceAttachment? attachment = await _unitOfWork.AttachmentRepository
            .GetByIdAsync(attachmentId) ?? throw new NotFoundException("Attachment not found");

        Stream stream = _fileStorageService.GetStream(attachment.FilePath);
        return (stream, attachment.FileName);
    }

    public async Task DeleteAsync(Guid attachmentId)
    {
        AttendanceAttachment? attachment = await _unitOfWork.AttachmentRepository
            .GetByIdAsync(attachmentId) ?? throw new NotFoundException("Attachment not found");
        
        _fileStorageService.Delete(attachment.FilePath);

        _unitOfWork.AttachmentRepository.Delete(attachment);
        await _unitOfWork.SaveChangesAsync();
    }

    private static void ValidateFile(IFormFile file)
    {
        if (file.Length > MaxFileSizeBytes)
            throw new ValidationException("File size cannot exceed 5MB");

        string extension = Path.GetExtension(file.FileName).ToLower();
        if (!AllowedExtensions.Contains(extension))
            throw new ValidationException("Only PDF, JPG, and PNG files are allowed");
    }
}