using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

        public async Task AddAsync(UserGameLibraryDTO userGameLibrary)
        {
            var userGameLibraryEntity = _mapper.Map<UserGameLibraryDTO, UserGameLibrary>(userGameLibrary);

            await _unitOfWork.UserGameLibraryRepository.InsertAsync(userGameLibraryEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var userGameLibrary = await _unitOfWork.UserGameLibraryRepository.GetByIdAsync(id);

            _unitOfWork.UserGameLibraryRepository.Delete(userGameLibrary);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveAsync(UserGameLibraryDTO userGameLibrary)
        {
            var userGameLibraryEntity = _mapper.Map<UserGameLibraryDTO, UserGameLibrary>(userGameLibrary);

            _unitOfWork.UserGameLibraryRepository.Delete(userGameLibraryEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserGameLibraryDTO>> GetAllAsync()
        {
            var userGameLibraries = await _unitOfWork.UserGameLibraryRepository.GetAllAsync().ToListAsync();

            return _mapper.Map<IEnumerable<UserGameLibrary>, IEnumerable<UserGameLibraryDTO>>(userGameLibraries);
        }

        public async Task<IEnumerable<UserGameLibraryDTO>> GetAllByUserIdAsync(string id)
        {
            var userGameLibraries = await _unitOfWork.UserGameLibraryRepository.GetAllAsync(ugl => ugl.ApplicationUserId == id).ToListAsync();

            return _mapper.Map<IEnumerable<UserGameLibrary>, IEnumerable<UserGameLibraryDTO>>(userGameLibraries);
        }

        public async Task<UserGameLibraryDTO> GetByIdAsync(int id)
        {
            var userGameLibrary = await _unitOfWork.UserGameLibraryRepository.GetByIdAsync(id);

            return _mapper.Map<UserGameLibrary, UserGameLibraryDTO>(userGameLibrary);
        }

        public async Task UpdateAsync(UserGameLibraryDTO userGameLibrary)
        {
            var userGameLibraryEntity = _mapper.Map<UserGameLibraryDTO, UserGameLibrary>(userGameLibrary);
            _unitOfWork.UserGameLibraryRepository.Update(userGameLibraryEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddGameToUserLibrary(int gameId, string userId)
        {
            var userGames = await GetAllByUserIdAsync(userId);
            var gameCheck = userGames.Where(g => g.GameId.Equals(gameId)).FirstOrDefault();

            if (gameCheck == null)
            {
                UserGameLibraryDTO userGameLibrary = new();
                userGameLibrary.GameId = gameId;
                userGameLibrary.ApplicationUserId = userId;
                await AddAsync(userGameLibrary);
            }
        }

        public async Task IsPassed(int id, bool isPassed)
        {
            UserGameLibraryDTO userGame = await GetByIdAsync(id);
            userGame.IsPassed = isPassed;
            await UpdateAsync(userGame);
        }
    }
}
