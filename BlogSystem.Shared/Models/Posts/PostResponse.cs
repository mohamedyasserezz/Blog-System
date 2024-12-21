using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Shared.Models.Posts
{
    public record PostResponse(
        string Id,
        string Title,
        string Content,
        ICollection<TagRequest> Tags,
        string AuthorId
        );
}
