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

namespace BLL.Infrastructure
{
    public class GenreService : IGenreService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(GenreDTO genre)
        {
            var genreEntity = _mapper.Map<GenreDTO, Genre>(genre);

            await _unitOfWork.GenreRepository.InsertAsync(genreEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var genre = await _unitOfWork.GenreRepository.GetByIdAsync(id);

            _unitOfWork.GenreRepository.Delete(genre);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<GenreDTO>> GetAllAsync()
        {
            var genres = await _unitOfWork.GenreRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<Genre>, IEnumerable<GenreDTO>>(genres);
        }

        public async Task<GenreDTO> GetByIdAsync(int id)
        {
            var genre = await _unitOfWork.GenreRepository.GetByIdAsync(id);

            return _mapper.Map<Genre, GenreDTO>(genre);
        }

        public async Task UpdateAsync(GenreDTO genre)
        {
            var genreEntity = _mapper.Map<GenreDTO, Genre>(genre);
            _unitOfWork.GenreRepository.Update(genreEntity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
