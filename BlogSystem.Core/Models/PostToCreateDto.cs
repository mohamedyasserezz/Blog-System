using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Models
{
    public record PostToCreateDto(
    string Title,
    string Content,
    string AuthorId
    );
}
