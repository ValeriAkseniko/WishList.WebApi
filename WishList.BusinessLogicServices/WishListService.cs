using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishList.BusinessLogic.Interfaces;
using WishList.DataAccess.Interfaces.Repositories;
using WishList.DataTransferObjects.WishListItems;
using WishList.DataTransferObjects.WishLists;
using WishList.Entities.Models;
using WishList.Infrastructure.Constants;
using WishList.Infrastructure.Exceptions;
using WishListDb = WishList.Entities.Models.WishList;

namespace WishList.BusinessLogicServices
{
    public class WishListService : IWishListService
    {
        private readonly IWishListRepository wishListRepository;
        private readonly IWishListItemRepository wishListItemRepository;
        private readonly IProfileRepository profileRepository;
        private readonly IRoleRepository roleRepository;
        private readonly ILogger<WishListService> logger;

        public WishListService(IWishListRepository wishListRepository, IWishListItemRepository wishListItemRepository, IProfileRepository profileRepository, IRoleRepository roleRepository, ILogger<WishListService> logger)
        {
            this.wishListRepository = wishListRepository;
            this.wishListItemRepository = wishListItemRepository;
            this.profileRepository = profileRepository;
            this.roleRepository = roleRepository;
            this.logger = logger;
        }

        public async Task CreateWishListAsync(WishListCreateRequest wishListCreateRequest, Guid profileId)
        {
            if (!await profileRepository.ProfileExistAsync(profileId))
            {
                var exception = new ProfileNotFoundException(profileId);
                logger.LogError(exception.ToString());
                throw exception;
            }
            var wishList = new WishListDb()
            {
                CreateDate = DateTime.Now,
                Description = wishListCreateRequest.Description,
                Id = Guid.NewGuid(),
                Name = wishListCreateRequest.Name,
                OwnerId = profileId
            };
            await wishListRepository.CreateAsync(wishList);
        }

        public async Task CreateWishListItemAsync(WishListItemsCreateRequest wishListItemsCreateRequest, Guid profileId)
        {
            if (!await profileRepository.ProfileExistAsync(profileId))
            {
                var exception = new ProfileNotFoundException(profileId);
                logger.LogError(exception.ToString());
                throw exception;
            }
            if (wishListItemsCreateRequest.WishListItems == null || wishListItemsCreateRequest.WishListItems.Count == 0)
            {
                return;
            }
            var wishlist = await wishListRepository.GetAsync(wishListItemsCreateRequest.WishListId);
            if (wishlist == null)
            {
                var exception = new WishListNotFoundException(wishListItemsCreateRequest.WishListId);
                logger.LogError(exception.ToString());
                throw exception;
            }
            if (profileId == wishlist.OwnerId)
            {
                var listItems = wishListItemsCreateRequest
                    .WishListItems
                    .Select(x => new ListItem
                    {
                        Id = Guid.NewGuid(),
                        Description = x.Description,
                        CreateDate = DateTime.Now,
                        WishListId = wishlist.Id,
                        Name = x.Name,
                        Price = x.Price,
                        Received = false,
                        Reference = x.Reference
                    })
                    .ToList();
                await wishListItemRepository.CreateAsync(listItems);
            }
            else
            {
                var exception = new AccessException();
                logger.LogError(exception.ToString());
                throw exception;
            }
        }

        public async Task DeleteAsync(Guid id, Guid profileId, Guid roleId)
        {
            if (!await profileRepository.ProfileExistAsync(profileId))
            {
                var exception = new ProfileNotFoundException(profileId);
                logger.LogError(exception.ToString());
                throw exception;
            }
            if (!await roleRepository.RoleExistAsync(roleId))
            {
                var exception = new RoleNotFoundException(roleId);
                logger.LogError(exception.ToString());
                throw exception;
            }
            var entity = await wishListRepository.GetAsync(id);
            if (entity == null)
            {
                var exception = new WishListNotFoundException(id);
                logger.LogError(exception.ToString());
                throw exception;
            }
            if (roleId == Permissions.Id.Admin || entity.OwnerId == profileId)
            {
                await wishListRepository.DeleteAsync(id);
            }
            else
            {
                var exception = new AccessException();
                logger.LogError(exception.ToString());
                throw exception;
            }
        }

        public async Task DeleteItemAsync(Guid id, Guid profileId, Guid roleId)
        {
            if (!await profileRepository.ProfileExistAsync(profileId))
            {
                var exception = new ProfileNotFoundException(profileId);
                logger.LogError(exception.ToString());
                throw exception;
            }
            if (!await roleRepository.RoleExistAsync(roleId))
            {
                var exception = new RoleNotFoundException(roleId);
                logger.LogError(exception.ToString());
                throw exception;
            }
            var item = await wishListItemRepository.GetAsync(id);
            if (item == null)
            {
                var exception = new WishListItemNotFoundException(id);
                logger.LogError(exception.ToString());
                throw exception;
            }
            var wishList = await wishListRepository.GetAsync(item.WishListId);
            if (wishList == null)
            {
                var exception = new WishListNotFoundException(item.WishListId);
                logger.LogError(exception.ToString());
                throw exception;
            }
            if (roleId == Permissions.Id.Admin || wishList.OwnerId == profileId)
            {
                await wishListItemRepository.DeleteAsync(id);
            }
            else
            {
                var exception = new AccessException();
                logger.LogError(exception.ToString());
                throw exception;
            }
        }

        public async Task<List<WishListItemView>> GetListItemsAsync(Guid wishListId, Guid profileId, Guid roleId)
        {
            if (!await profileRepository.ProfileExistAsync(profileId))
            {
                var exception = new ProfileNotFoundException(profileId);
                logger.LogError(exception.ToString());
                throw exception;
            }
            if (!await roleRepository.RoleExistAsync(roleId))
            {
                var exception = new RoleNotFoundException(roleId);
                logger.LogError(exception.ToString());
                throw exception;
            }
            var wishlist = await wishListRepository.GetAsync(wishListId);
            if (wishlist == null)
            {
                var exception = new WishListNotFoundException(wishListId);
                logger.LogError(exception.ToString());
                throw exception;
            }
            if (roleId == Permissions.Id.Admin || profileId == wishlist.OwnerId)
            {
                return wishlist.ListItems
                    .Select(x => new WishListItemView
                    {
                        Description = x.Description,
                        Id = x.Id,
                        Name = x.Name,
                        Received = x.Received,
                        Reference = x.Reference
                    })
                    .ToList();
            }
            else
            {
                var exception = new AccessException();
                logger.LogError(exception.ToString());
                throw exception;
            }
        }

        public async Task<WishListView> GetWishListAsync(Guid id)
        {
            var item = await wishListRepository.GetAsync(id);
            if (item == null)
            {
                var exception = new WishListNotFoundException(id);
                logger.LogError(exception.ToString());
                throw exception;
            }
            var newItem = new WishListView()
            {
                Description = item.Description,
                Id = item.Id,
                ListItems = item.ListItems
                .Select(x => new WishListItemView
                {
                    Description = x.Description,
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Received = x.Received,
                    Reference = x.Reference
                })
                .ToList(),
                Name = item.Name
            };
            return newItem;
        }

        public async Task<List<WishListView>> GetWishListByOwnerAsync(Guid ownerId)
        {
            var wishLists = await wishListRepository.ListAsync(ownerId);
            if (wishLists == null)
            {
                var exception = new WishListByOwnerNotFound(ownerId);
                logger.LogError(exception.ToString());
                throw exception;
            }
            return wishLists
                .Select(x => new WishListView
                {
                    Id = x.Id,
                    Description = x.Description,
                    ListItems = x.ListItems
                    .Select(li => new WishListItemView
                    {
                        Description = li.Description,
                        Id = li.Id,
                        Name = li.Name,
                        Price = li.Price,
                        Received = li.Received,
                        Reference = li.Reference
                    })
                    .ToList(),
                    Name = x.Name
                })
                .ToList();
        }

        public async Task<WishListItemView> GetWishListItemAsync(Guid id, Guid profileId, Guid roleId)
        {
            if (!await profileRepository.ProfileExistAsync(profileId))
            {
                var exception = new ProfileNotFoundException(profileId);
                logger.LogError(exception.ToString());
                throw exception;
            }
            if (!await roleRepository.RoleExistAsync(roleId))
            {
                var exception = new RoleNotFoundException(roleId);
                logger.LogError(exception.ToString());
                throw exception;
            }
            var listItem = await wishListItemRepository.GetAsync(id);
            if (listItem == null)
            {
                var exception = new WishListItemNotFoundException(id);
                logger.LogError(exception.ToString());
                throw exception;
            }
            var wishlist = await wishListRepository.GetAsync(listItem.WishListId);
            if (wishlist == null)
            {
                var exception = new WishListNotFoundException(listItem.WishListId);
                logger.LogError(exception.ToString());
                throw exception;
            }
            if (profileId == wishlist.OwnerId || roleId == Permissions.Id.Admin)
            {
                var itemView = new WishListItemView
                {
                    Description = listItem.Description,
                    Id = listItem.Id,
                    Name = listItem.Name,
                    Received = listItem.Received,
                    Reference = listItem.Reference
                };
                return itemView;
            }
            else
            {
                var exception = new AccessException();
                logger.LogError(exception.ToString());
                throw exception;
            }
        }

        public async Task<List<WishListView>> GetWishListsAsync()
        {
            var entities = await wishListRepository.ListAsync();
            return entities
                .Select(x => new WishListView
                {
                    Id = x.Id,
                    Description = x.Description,
                    ListItems = x.ListItems
                    .Select(li => new WishListItemView
                    {
                        Description = li.Description,
                        Id = li.Id,
                        Name = li.Name,
                        Price = li.Price,
                        Received = li.Received,
                        Reference = li.Reference
                    })
                    .ToList(),
                    Name = x.Name
                })
                .ToList();
        }
    }
}
