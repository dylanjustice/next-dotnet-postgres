using System;

namespace DylanJustice.Demo.Presentation.Web.Models.Dtos
{
    public class CreatableDto : EntityDto
    {
        public long? CreatedById { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }

    }
}