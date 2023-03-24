using AutoMapper;
using BLL.DTO;
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
        IUnitOfWork _unitOfWork;
        IMapper _mapper;

        public UserGameLibraryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(UserGameDTO userGameLibrary)
        {
            var userGameLibraryEntity = _mapper.Map<UserGameDTO, UserGame>(userGameLibrary);

            await _unitOfWork.UserGameLibraryRepository.InsertAsync(userGameLibraryEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var userGameLibrary = await _unitOfWork.UserGameLibraryRepository.GetByIdAsync(id);

            _unitOfWork.UserGameLibraryRepository.Delete(userGameLibrary);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveAsync(UserGameDTO userGameLibrary)
        {
            var userGameLibraryEntity = _mapper.Map<UserGameDTO, UserGame>(userGameLibrary);

            _unitOfWork.UserGameLibraryRepository.Delete(userGameLibraryEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserGameDTO>> GetAllAsync()
        {
            var userGameLibraries = await _unitOfWork.UserGameLibraryRepository.GetAllWithIncludes().ToListAsync();

            return _mapper.Map<IEnumerable<UserGame>, IEnumerable<UserGameDTO>>(userGameLibraries);
        }

        public async Task<IEnumerable<UserGameDTO>> GetAllByUserIdAsync(string id)
        {
            var userGameLibraries = await _unitOfWork.UserGameLibraryRepository.GetAllWithIncludes(ugl => ugl.ApplicationUserId == id).ToListAsync();

            return _mapper.Map<IEnumerable<UserGame>, IEnumerable<UserGameDTO>>(userGameLibraries);
        }

        public async Task<UserGameDTO> GetByIdAsync(int id)
        {
            var userGameLibrary = await _unitOfWork.UserGameLibraryRepository.GetByIdAsync(id);

            return _mapper.Map<UserGame, UserGameDTO>(userGameLibrary);
        }

        public async Task UpdateAsync(UserGameDTO userGameLibrary)
        {
            var userGameLibraryEntity = _mapper.Map<UserGameDTO, UserGame>(userGameLibrary);
            _unitOfWork.UserGameLibraryRepository.Update(userGameLibraryEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddGameToUserLibraryAsync(int gameId, string userId)
        {
            var userGames = await GetAllByUserIdAsync(userId);
            var gameCheck = userGames.Where(g => g.GameId.Equals(gameId)).FirstOrDefault();

            if (gameCheck == null)
            {
                UserGameDTO userGameLibrary = new();
                userGameLibrary.GameId = gameId;
                userGameLibrary.ApplicationUserId = userId;
                await AddAsync(userGameLibrary);
            }
        }

        public async Task IsGamePassedAsync(int id, bool isPassed)
        {
            UserGameDTO userGame = await GetByIdAsync(id);
            userGame.IsPassed = isPassed;
            await UpdateAsync(userGame);
        }
    }
}
