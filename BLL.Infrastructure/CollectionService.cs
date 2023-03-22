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
    public class CollectionService : ICollectionService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;

        public CollectionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(CollectionDTO collection)
        {
            var collectionEntity = _mapper.Map<CollectionDTO, Collection>(collection);

            await _unitOfWork.CollectionRepository.InsertAsync(collectionEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var collection = await _unitOfWork.CollectionRepository.GetByIdAsync(id);

            _unitOfWork.CollectionRepository.Delete(collection);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveAsync(CollectionDTO collection)
        {
            var collectionEntity = _mapper.Map<CollectionDTO, Collection>(collection);

            _unitOfWork.CollectionRepository.Delete(collectionEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CollectionDTO>> GetAllAsync()
        {
            var collections = await _unitOfWork.CollectionRepository.GetAllAsync().ToListAsync();

            return _mapper.Map<IEnumerable<Collection>, IEnumerable<CollectionDTO>>(collections);
        }

        public async Task<CollectionDTO> GetByIdAsync(int id)
        {
            var collection = await _unitOfWork.CollectionRepository.GetByIdAsync(id);

            return _mapper.Map<Collection, CollectionDTO>(collection);
        }

        public async Task UpdateAsync(CollectionDTO collection)
        {
            var collectionEntity = _mapper.Map<CollectionDTO, Collection>(collection);
            _unitOfWork.CollectionRepository.Update(collectionEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        //public async Task<IEnumerable<CollectionDTO>> Search(string searchString)
        //{
        //    var collections = await GetAllAsync();

        //    return collections.Where(g => g.Name.ToUpper().Contains(searchString.ToUpper()));
        //}

        //public async Task<IEnumerable<CollectionDTO>> FilterByGenre(string collectionGenre)
        //{
        //    var collections = await GetAllAsync();
        //    var result = collections.Where(g => g.Genre != null && g.Genre.ToUpper().Contains(collectionGenre.ToUpper()));
        //    return result;
        //}

        public async Task AddGamesToCollectionAsync(int CollectionId, List<string> SelectedGames)
        {
            var userCollectionGames = _unitOfWork.UserCollectionRepository.GetAllAsync();            

            for (int i = 0; i < SelectedGames.Count; i++)
            {
                UserCollectionDTO userCollection = new();

                userCollection.CollectionId = CollectionId; // Add CollectionID

                int GameId = int.Parse(SelectedGames[i]);
                userCollection.UserGameId = GameId; // Add UserGameID

                var UserCollectionEntity = _mapper.Map<UserCollectionDTO, UserCollection>(userCollection);

                var duplicateUserGameCheck = userCollectionGames.Where(uc => uc.UserGameId == GameId && uc.CollectionId == CollectionId).FirstOrDefault();
                if (duplicateUserGameCheck == null)
                {
                    await _unitOfWork.UserCollectionRepository.InsertAsync(UserCollectionEntity); // Add created collection to DB
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CollectionDTO>> GetAllByUserIdAsync(string UserId)
        {
            var userCollections = await _unitOfWork.CollectionRepository.GetAllAsync().ToListAsync();

            var userCollectionsByUserId = userCollections.Where(x => x.ApplicationUserId == UserId);

            return _mapper.Map<IEnumerable<Collection>, IEnumerable<CollectionDTO>>(userCollectionsByUserId);
        }
    }
}
