﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BreakoutExtreme.Components
{
    public partial class PlayArea
    {
        private static readonly ReadOnlyDictionary<Levels, string> _levelAreas = new(new Dictionary<Levels, string>()
        {
            {
                Levels.Beginner0,
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "____L__________L______" +
                "_______L__M_L_________" +
                "______________________" +
                "__________SS__________" +
                "______________________" +
                "______________________" +
                "_L____M_______M___L___" +
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
                "______________________" +
                "______________________" +
                "______________________"
            },
            {
                Levels.Beginner1,
                "______________________" +
                "______________________" +
                "__S_______C________S__" +
                "___S______________S___" +
                "____S____M_M_____S____" +
                "_____S__L__L____S_____" +
                "_____S__________S_____" +
                "______________________" +
                "______________________" +
                "_M_M___M_M_M_M___M_M__" +
                "________L__L__________" +
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
                "______________________" +
                "______________________" +
                "__________o___________" +
                "________P_____________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________"
            },
            {
                Levels.Beginner2,
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "_____0___________0____" +
                "_____SL__S1SL__L__L___" +
                "_____L__L__L__L__L__S_" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "____0___________0_____" +
                "_M_L__S1SL__L__M______" +
                "_SL__L__L__L__L_______" +
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
                "______________________" +
                "______________________" +
                "______________________"
            },
            {
                Levels.Beginner3,
                "______________________" +
                "______________________" +
                "__C_______________C___" +
                "______________________" +
                "__M_______________M___" +
                "______________________" +
                "______M_______M_______" +
                "______________________" +
                "______________________" +
                "___S______________S___" +
                "___M_____________2S___" +
                "____M___________M_____" +
                "________S____S________" +
                "_L______M___M_____L___" +
                "__L__L__L__L__L__L____" +
                "________L__S4S________" +
                "_________S4M__________" +
                "_________M_M__________" +
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
                "______________________" +
                "______________________" +
                "______________________"
            },
            {
                Levels.Beginner4,
                "______________________" +
                "______________________" +
                "_______C_____C________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "_____S__________S_____" +
                "_____S__________S_____" +
                "_____M_________M______" +
                "_____M_________M______" +
                "______________________" +
                "_______L____L_________" +
                "______________________" +
                "____S3_________0S_____" +
                "____M___________M_____" +
                "___L____________L_____" +
                "______________________" +
                "__M_______________M___" +
                "__M_______S0______M___" +
                "_L_______M_M______L___" +
                "_________M_M__________" +
                "__________3S__________" +
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
                "______________________" +
                "______________________" +
                "______________________"
            },
            {
                Levels.Loop0,
                "______________________" +
                "______________________" +
                "_____C_________C______" +
                "______________________" +
                "______________________" +
                "_______M_____M________" +
                "______S________S______" +
                "_____S__________S_____" +
                "____S____________S____" +
                "_L________S0______M_2_" +
                "__S______M_M_______S__" +
                "_S__________________S_" +
                "______________________" +
                "________M_0L__________" +
                "______________________" +
                "_M_________________M__" +
                "_L_______M_M______L___" +
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
                "______________________" +
                "_L________________L___" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________"
            },
            {
                Levels.Loop1,
                "______________________" +
                "______________________" +
                "___________C__________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "___C______SSSS________" +
                "__________SSSS________" +
                "__SSSS____SSSS________" +
                "__SSSS_0______________" +
                "__SSSS________________" +
                "___________0__________" +
                "______________________" +
                "_______________0______" +
                "______________________" +
                "__________________L___" +
                "__________________L___" +
                "________________L__M__" +
                "______________L__L__S_" +
                "___________SSSL__L____" +
                "________3M_SSS1SM_M___" +
                "_____L__L__SSSL_______" +
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
                "______________________" +
                "______________________" +
                "______________________"
            },
            {
                Levels.Loop2,
                "______________________" +
                "______________________" +
                "__________C___________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______S3S____L________" +
                "_______M_____M________" +
                "______________________" +
                "______________________" +
                "________S____1________" +
                "________S____S________" +
                "___L____1____S__L_____" +
                "____M___________M_____" +
                "_____S__________S_____" +
                "_____S__________S_____" +
                "_____S__________S_____" +
                "______________________" +
                "______________________" +
                "______________________" +
                "__________o___________" +
                "________P_____________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______S________S______" +
                "______M_0____0M_______" +
                "________L__L__________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________"
            },
            {
                Levels.Loop3,
                "______________________" +
                "______________________" +
                "__________C___________" +
                "______________________" +
                "__C_______M_______C___" +
                "__________M___________" +
                "__M_____L__L______M___" +
                "__M____L__00L_____M___" +
                "__M____L__00L_____M___" +
                "__M____L____L_____M___" +
                "__S4___L____L_____3S__" +
                "__M_____M___S1____M___" +
                "__M_____M___M_____M___" +
                "_L_______M_M______L___" +
                "_________M_M__________" +
                "__________SS__________" +
                "__________SS__________" +
                "_________S__S_________" +
                "_______M_____M________" +
                "______M_______M_______" +
                "_____S__________S_____" +
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
                "______________________" +
                "______________________" +
                "______________________"
            },
            {
                Levels.Test,
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "_________L____________" +
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
            },
            {
                Levels.Test2,
                "______________________" +
                "_________L____________" +
                "______________________" +
                "______________________" +
                "________C_____________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "_____________L________" +
                "______S_______________" +
                "______________________" +
                "___0_______3__________" +
                "________________1_____" +
                "______________________" +
                "__________4___________" +
                "_______________2______" +
                "_______M_M____________" +
                "_______________M______" +
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
            },
            {
                Levels.Test3,
                "______________________" +
                "______________________" +
                "______________________" +
                "________C_____________" +
                "_______L______________" +
                "____________0_________" +
                "______________________" +
                "______________________" +
                "__________0__L__L_____" +
                "_____0________________" +
                "______________________" +
                "____L_________________" +
                "_____L________________" +
                "______________________" +
                "______________________" +
                "______________________" +
                "_____4________________" +
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
        private static readonly ReadOnlyDictionary<Levels, string> _levelTutorialMessages = new(new Dictionary<Levels, string>() 
        {
            { Levels.Beginner0, "Touch (click) and hold to move the paddle!\nTap (click) the Paddle to start!" },
            { Levels.Beginner1, "Rapidly tap (click) to fire laser and destroy bombs!" },
        });
    }
}
