using AutoMapper;
using BLL.DTO;
using BLL.Infrastructure.Exceptions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public class CollectionService : ICollectionService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public CollectionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(CollectionDTO collectionDTO)
        {
            collectionDTO.Date = DateOnly.FromDateTime(DateTime.Now);

            var collection = _mapper.Map<CollectionDTO, Collection>(collectionDTO);

            await _unitOfWork.CollectionRepository.InsertAsync(collection);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var collection = await _unitOfWork.CollectionRepository.GetByIdAsync(id);

            _unitOfWork.CollectionRepository.Delete(collection);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveAsync(CollectionDTO collectionDTO)
        {
            var collection = _mapper.Map<CollectionDTO, Collection>(collectionDTO);

            _unitOfWork.CollectionRepository.Delete(collection);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CollectionDTO>> GetAllAsync()
        {
            var collections = await _unitOfWork.CollectionRepository.GetAllWithIncludes().ToListAsync();

            return _mapper.Map<IEnumerable<Collection>, IEnumerable<CollectionDTO>>(collections);
        }

        public async Task<CollectionDTO> GetByIdAsync(int id)
        {
            var collection = await _unitOfWork.CollectionRepository.GetByIdAsync(id)
                ?? throw new ObjectNotFoundException(nameof(Collection), id);

            return _mapper.Map<Collection, CollectionDTO>(collection);
        }

        public async Task UpdateAsync(CollectionDTO collectionDTO)
        {
            var collection = _mapper.Map<CollectionDTO, Collection>(collectionDTO);

            _unitOfWork.CollectionRepository.Update(collection);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddGamesToCollectionAsync(int collectionId, int[] selectedGames)
        {
            var usersCollections = _unitOfWork.UserCollectionRepository.GetAllWithIncludes();

            for (int i = 0; i < selectedGames.Length; i++)
            {
                int gameId = selectedGames[i];

                UserCollectionDTO userCollectionDTO = new()
                {
                    CollectionId = collectionId,
                    UserGameId = gameId
                };

                var userCollection = _mapper.Map<UserCollectionDTO, UserCollection>(userCollectionDTO);

                bool isGameInUserCollection = usersCollections
                    .Where(uc => uc.UserGameId == gameId && uc.CollectionId == collectionId)
                    .Any();

                if (!isGameInUserCollection)
                {
                    await _unitOfWork.UserCollectionRepository.InsertAsync(userCollection);
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CollectionDTO>> GetAllByUserIdAsync(string userId)
        {
            var collections = await _unitOfWork.CollectionRepository.GetAllWithIncludes().ToListAsync();

            var userCollections = collections.Where(x => x.ApplicationUserId == userId);

            return _mapper.Map<IEnumerable<Collection>, IEnumerable<CollectionDTO>>(userCollections);
        }

        public async Task RemoveGameFromCollectionAsync(UserCollectionDTO userCollectionDTO)
        {
            var userCollection = _mapper.Map<UserCollectionDTO, UserCollection>(userCollectionDTO);

            _unitOfWork.UserCollectionRepository.Delete(userCollection);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
