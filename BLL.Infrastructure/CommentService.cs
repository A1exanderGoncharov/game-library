using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BLL.Infrastructure
{
    public class CommentService : ICommentService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(CommentDTO comment)
        {
            var commentEntity = _mapper.Map<CommentDTO, Comment>(comment);
            commentEntity.Date = DateTime.Now;

            await _unitOfWork.CommentRepository.InsertAsync(commentEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id);

            //var comments = await _unitOfWork.CommentRepository.DbsetWithProperties().ToListAsync();
            //var comment = comments.FirstOrDefault(c => c.Id == id);

            _unitOfWork.CommentRepository.Delete(comment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveAsync(CommentDTO comment)
        {
            var commentEntity = _mapper.Map<CommentDTO, Comment>(comment);

            _unitOfWork.CommentRepository.Delete(commentEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommentDTO>> GetAllAsync()
        {
            var comments = await _unitOfWork.CommentRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(comments);
        }

        public async Task<CommentDTO> GetByIdAsync(int id)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id);

            return _mapper.Map<Comment, CommentDTO>(comment);
        }

        public async Task UpdateAsync(CommentDTO comment)
        {
            var commentEntity = _mapper.Map<CommentDTO, Comment>(comment);
            _unitOfWork.CommentRepository.Update(commentEntity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
