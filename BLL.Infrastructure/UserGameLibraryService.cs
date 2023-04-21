using AutoMapper;
using BLL.DTO;
using BLL.Infrastructure.Exceptions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public class UserGameLibraryService : IUserGameLibraryService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public UserGameLibraryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(UserGameDTO userGameDTO)
        {
            var userGame = _mapper.Map<UserGameDTO, UserGame>(userGameDTO);

            await _unitOfWork.UserGameLibraryRepository.InsertAsync(userGame);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var userGame = await _unitOfWork.UserGameLibraryRepository.GetByIdAsync(id);

            _unitOfWork.UserGameLibraryRepository.Delete(userGame);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveAsync(UserGameDTO userGameDTO)
        {
            var userGame = _mapper.Map<UserGameDTO, UserGame>(userGameDTO);

            _unitOfWork.UserGameLibraryRepository.Delete(userGame);
            await _unitOfWork.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<UserGameDTO>> GetAllAsync()
        {
            var usersGames = await _unitOfWork.UserGameLibraryRepository.GetAllWithIncludes().ToListAsync();

            return _mapper.Map<IEnumerable<UserGame>, IEnumerable<UserGameDTO>>(usersGames);
        }

        public async Task<IEnumerable<UserGameDTO>> GetAllByUserIdAsync(string id)
        {
            var userGameLibraries = await _unitOfWork.UserGameLibraryRepository
                .GetAllWithIncludes(ug => ug.ApplicationUserId == id).ToListAsync();

            return _mapper.Map<IEnumerable<UserGame>, IEnumerable<UserGameDTO>>(userGameLibraries);
        }

        public async Task<UserGameDTO> GetByIdAsync(int id)
        {
            var userGame = await _unitOfWork.UserGameLibraryRepository.GetByIdAsync(id)
                ?? throw new ElementNotFoundException(nameof(UserGame), id);

            return _mapper.Map<UserGame, UserGameDTO>(userGame);
        }

        public async Task UpdateAsync(UserGameDTO userGameDTO)
        {
            var userGame = _mapper.Map<UserGameDTO, UserGame>(userGameDTO);

            _unitOfWork.UserGameLibraryRepository.Update(userGame);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddGameToUserLibraryAsync(int gameId, string userId)
        {
            var userGamesDTO = await GetAllByUserIdAsync(userId);

            bool isGameInUserLibrary = userGamesDTO.Any(g => g.GameId.Equals(gameId));

            if (!isGameInUserLibrary)
            {
                UserGameDTO userGameDTO = new()
                {
                    GameId = gameId,
                    ApplicationUserId = userId
                };
                await AddAsync(userGameDTO);
            }
        }

        public async Task IsGamePassedAsync(int id, bool isPassed)
        {
            UserGameDTO userGameDTO = await GetByIdAsync(id);
            userGameDTO.IsPassed = isPassed;

            await UpdateAsync(userGameDTO);
        }
    }
}
