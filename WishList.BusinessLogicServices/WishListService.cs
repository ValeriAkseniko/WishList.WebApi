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
            if (await profileRepository.ProfileExistAsync(profileId))
            {
                logger.LogError(new ProfileNotFoundException(profileId).ToString());
                throw new ProfileNotFoundException(profileId);
            }
            WishListDb wishList = new WishListDb()
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
            if (await profileRepository.ProfileExistAsync(profileId))
            {
                logger.LogError(new ProfileNotFoundException(profileId).ToString());
                throw new ProfileNotFoundException(profileId);
            }
            if (wishListItemsCreateRequest.WishListItems == null || wishListItemsCreateRequest.WishListItems.Count == 0)
            {
                return;
            }
            var wishlist = await wishListRepository.GetAsync(wishListItemsCreateRequest.WishListId);
            if (profileId == wishlist.OwnerId)
            {
                List<ListItem> listItems = wishListItemsCreateRequest
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
                logger.LogError(new AccessException().ToString());
                throw new AccessException();
            }
        }

        public async Task DeleteAsync(Guid id, Guid profileId, Guid roleId)
        {
            if (await profileRepository.ProfileExistAsync(profileId))
            {
                logger.LogError(new ProfileNotFoundException(profileId).ToString());
                throw new ProfileNotFoundException(profileId);
            }
            if (await roleRepository.RoleExistAsync(roleId))
            {
                logger.LogError(new RoleNotFoundException(roleId).ToString());
                throw new RoleNotFoundException(roleId);
            }
            if (await wishListRepository.WishListExistAsync(id))
            {
                logger.LogError(new WishListNotFoundException(id).ToString());
                throw new WishListNotFoundException(id);
            }
            var item = await wishListRepository.GetAsync(id);
            if (roleId == Permissions.Id.Admin || item.OwnerId == profileId)
            {
                await wishListRepository.DeleteAsync(id);
            }
            else
            {
                logger.LogError(new AccessException().ToString());
                throw new AccessException();
            }
        }

        public async Task DeleteItemAsync(Guid id, Guid profileId, Guid roleId)
        {
            if (await profileRepository.ProfileExistAsync(profileId))
            {
                logger.LogError(new ProfileNotFoundException(profileId).ToString());
                throw new ProfileNotFoundException(profileId);
            }
            if (await roleRepository.RoleExistAsync(roleId))
            {
                logger.LogError(new RoleNotFoundException(roleId).ToString());
                throw new RoleNotFoundException(roleId);
            }
            if (await wishListItemRepository.WishListItemExistAsync(id))
            {
                logger.LogError(new WishListItemNotFoundException(id).ToString());
                throw new WishListItemNotFoundException(id);
            }
            var item = await wishListItemRepository.GetAsync(id);
            if (await wishListRepository.WishListExistAsync(item.WishListId))
            {
                logger.LogError(new WishListNotFoundException(item.WishListId).ToString());
                throw new WishListNotFoundException(item.WishListId);
            }
            var wishList = await wishListRepository.GetAsync(item.WishListId);            
            if (roleId == Permissions.Id.Admin || wishList.OwnerId == profileId)
            {
                await wishListItemRepository.DeleteAsync(id);
            }
            else
            {
                logger.LogError(new AccessException().ToString());
                throw new AccessException();
            }
        }

        public async Task<List<WishListItemView>> GetListItemsAsync(Guid wishListId, Guid profileId, Guid roleId)
        {
            if (await profileRepository.ProfileExistAsync(profileId))
            {
                logger.LogError(new ProfileNotFoundException(profileId).ToString());
                throw new ProfileNotFoundException(profileId);
            }
            if (await roleRepository.RoleExistAsync(roleId))
            {
                logger.LogError(new RoleNotFoundException(roleId).ToString());
                throw new RoleNotFoundException(roleId);
            }
            if (await wishListRepository.WishListExistAsync(wishListId))
            {
                logger.LogError(new WishListNotFoundException(wishListId).ToString());
                throw new WishListNotFoundException(wishListId);
            }
            var wishlist = await wishListRepository.GetAsync(wishListId);
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
                logger.LogError(new AccessException().ToString());
                throw new AccessException();
            }
        }

        public async Task<WishListView> GetWishListAsync(Guid id)
        {
            if (await wishListRepository.WishListExistAsync(id))
            {
                logger.LogError(new WishListNotFoundException(id).ToString());
                throw new WishListNotFoundException(id);
            }
            var item = await wishListRepository.GetAsync(id);
            WishListView newItem = new WishListView()
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
            if (await profileRepository.ProfileExistAsync(ownerId))
            {
                logger.LogError(new ProfileNotFoundException(ownerId).ToString());
                throw new ProfileNotFoundException(ownerId);
            }
            var wishLists = await wishListRepository.ListAsync(ownerId);
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
            if (await profileRepository.ProfileExistAsync(profileId))
            {
                logger.LogError(new ProfileNotFoundException(profileId).ToString());
                throw new ProfileNotFoundException(profileId);
            }
            if (await roleRepository.RoleExistAsync(roleId))
            {
                logger.LogError(new RoleNotFoundException(roleId).ToString());
                throw new RoleNotFoundException(roleId);
            }
            if (await wishListItemRepository.WishListItemExistAsync(id))
            {
                logger.LogError(new WishListItemNotFoundException(id).ToString());
                throw new WishListItemNotFoundException(id);
            }
            var listItem = await wishListItemRepository.GetAsync(id);
            var wishlist = await wishListRepository.GetAsync(listItem.WishListId);
            if (profileId == wishlist.OwnerId || roleId == Permissions.Id.Admin)
            {
                WishListItemView itemView = new WishListItemView
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
                logger.LogError(new AccessException().ToString());
                throw new AccessException();
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
