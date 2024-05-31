using AutoMapper;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using MiTube.BLL.DTO;
using MiTube.BLL.Interfaces;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;
using MiTube.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMapper mapper;
        private readonly ICommentRepository repo;
        private readonly IVideoService videoService;
        private readonly IUserService userService;

        public CommentService(IMapper mapper, ICommentRepository repo, IVideoService videoService, IUserService userService)
        {
            this.mapper = mapper;
            this.repo = repo;
            this.videoService = videoService;
            this.userService = userService;
        }

        public Task<IEnumerable<CommentDTO>> SearchAsync(string search)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CommentDTO>> GetAllAsync()
        {
            IEnumerable<Comment> comments = await repo.GetAllAsync();
            return comments == null ?
                Enumerable.Empty<CommentDTO>() :
                mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(comments);
        }

        public async Task<CommentDTO?> GetByIdAsync(Guid id)
        {
            Comment? comment = await repo.GetByIdAsync(id);

            return comment == null ? null : mapper.Map<Comment, CommentDTO>(comment);
        }

        public async Task<CommentDTO?> CreateAsync(CommentDTO commentDto)
        {
            commentDto.Id = Guid.NewGuid();

            Comment comment = mapper.Map<CommentDTO, Comment>(commentDto);

            if (await userService.GetByIdAsync(comment.UserId) == null ||
                await videoService.GetByIdAsync(comment.VideoId, Guid.NewGuid()) == null)
                return null;

            await repo.CreateAsync(comment);

            return mapper.Map<Comment, CommentDTO>(comment);
        }

        public async Task<CommentDTO?> UpdateAsync(CommentDTO commentDto)
        {
            Comment incoming = mapper.Map<CommentDTO, Comment>(commentDto);
            Comment? inbound = await repo.GetByIdWithEverythingAsync(commentDto.Id);

            if (inbound == null || incoming.UserId != inbound.UserId || incoming.VideoId != inbound.VideoId) return null;
            
            return mapper.Map<Comment, CommentDTO>(
                    inbound.UserId != incoming.UserId ?
                    inbound :
                    await repo.UpdateAsync(incoming)
                    );
        }

        public async Task DeleteAsync(Guid id)
        {
            CommentDTO? commentDto = await GetByIdAsync(id);

            if (commentDto != null)
                await repo.DeleteAsync(mapper.Map<CommentDTO, Comment>(commentDto));
        }

        public async Task DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CommentDTO>> GetByVideoIdAsync(Guid id)
        {
            
            IEnumerable<Comment> comments = await repo.FindByConditionAsync(c => c.VideoId == id);
            return mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(comments);
        }

        public async Task<IEnumerable<CommentDTO>> GetByUserIdAsync(Guid id)
        {
            IEnumerable<Comment> comments = await repo.FindByConditionAsync(c => c.UserId == id);
            return mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(comments);
        }
    }
}
