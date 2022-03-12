using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.Enums

{
    public enum AudienceType
    {
        /// <summary>
        /// малая комната проведения занятий
        /// </summary>
        [Description("Классная комната")]
        ClassRoom = 1,
        /// <summary>
        /// большая комната проведения занятий
        /// </summary>
        [Description("Аудитория")]
        Audience = 2,
        /// <summary>
        /// актовый зал
        /// </summary>
        [Description("Актовый зал")]
        AssemblyHall = 3
    }
}
