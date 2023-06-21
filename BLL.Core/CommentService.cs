using AutoMapper;
using BLL.DTO;
using BLL.Core.Exceptions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Core
{
    public class CommentService : ICommentService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(CommentDTO commentDTO)
        {
            commentDTO.Date = DateTime.UtcNow;
            var comment = _mapper.Map<CommentDTO, Comment>(commentDTO);

            await _unitOfWork.CommentRepository.InsertAsync(comment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id);

            _unitOfWork.CommentRepository.Delete(comment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveAsync(CommentDTO commentDTO)
        {
            var comment = _mapper.Map<CommentDTO, Comment>(commentDTO);

            _unitOfWork.CommentRepository.Delete(comment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommentDTO>> GetAllAsync()
        {
            var comments = await _unitOfWork.CommentRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(comments);
        }

        public async Task<CommentDTO> GetByIdAsync(int id)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id)
                ?? throw new ObjectNotFoundException(nameof(Comment), id);

            return _mapper.Map<Comment, CommentDTO>(comment);
        }

        public async Task UpdateAsync(CommentDTO commentDTO)
        {
            var comment = _mapper.Map<CommentDTO, Comment>(commentDTO);

            _unitOfWork.CommentRepository.Update(comment);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
