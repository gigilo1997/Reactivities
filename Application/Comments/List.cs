using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comments;
public class List {
  public record Query(Guid ActivityId) : IRequest<Result<List<CommentDto>>>;

  public class Handler : IRequestHandler<Query, Result<List<CommentDto>>>
  {
    private readonly ReactivityContext context;
    private readonly IMapper mapper;

    public Handler(ReactivityContext context, IMapper mapper)
    {
      this.context = context;
      this.mapper = mapper;
    }

    public async Task<Result<List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
    {
      var comments = await context.Comments
        .Where(x => x.Activity.Id == request.ActivityId)
        .OrderByDescending(x => x.CreatedAt)
        .ProjectTo<CommentDto>(mapper.ConfigurationProvider)
        .ToListAsync();

      return Result<List<CommentDto>>.Success(comments);
    }
  }
}
