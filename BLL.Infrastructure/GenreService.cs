using AutoMapper;
using BLL.DTO;
using BLL.Infrastructure.Exceptions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public class GenreService : IGenreService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(GenreDTO genreDTO)
        {
            var genre = _mapper.Map<GenreDTO, Genre>(genreDTO);

            await _unitOfWork.GenreRepository.InsertAsync(genre);
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

            return _mapper.Map<IEnumerable<Genre>, IEnumerable<GenreDTO>>(genres.OrderBy(g => g.Name));
        }

        public async Task<GenreDTO> GetByIdAsync(int id)
        {
            var genre = await _unitOfWork.GenreRepository.GetByIdAsync(id)
                ?? throw new ElementNotFoundException(nameof(Genre), id);

            return _mapper.Map<Genre, GenreDTO>(genre);
        }

        public async Task UpdateAsync(GenreDTO genreDTO)
        {
            var genre = _mapper.Map<GenreDTO, Genre>(genreDTO);

            _unitOfWork.GenreRepository.Update(genre);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<GenreDTO>> GetAllGenresOrderedByAsync()
        {
            var genres = await _unitOfWork.GenreRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<Genre>, IEnumerable<GenreDTO>>(genres).OrderBy(g => g.Name).ToList();
        }
    }
}
