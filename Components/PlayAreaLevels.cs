﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BreakoutExtreme.Components
{
    public partial class PlayArea
    {
        private static readonly ReadOnlyDictionary<Levels, string> _levelAreas = new(new Dictionary<Levels, string>()
        {
            {
                Levels.Test,
                "______________________" +
                "_______________B______" +
                "______________________" +
                "______________________" +
                "________B_______C_____" +
                "______________________" +
                "___C__________________" +
                "_____________B________" +
                "______________________" +
                "_B___B___B___B____B___" +
                "______________________" +
                "______________________" +
                "______B_______________" +
                "______________________" +
                "_____________B________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "__________o___________" +
                "________P_____________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________"
            }
        });
    }
}
