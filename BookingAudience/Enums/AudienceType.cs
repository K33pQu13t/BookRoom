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
        ClassRoom,
        /// <summary>
        /// большая комната проведения занятий
        /// </summary>
        [Description("Аудитория")]
        Audience,
        /// <summary>
        /// актовый зал
        /// </summary>
        [Description("Актовый зал")]
        AssemblyHall
    }
}
