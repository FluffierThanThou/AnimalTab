﻿using UnityEngine;
using Verse;

namespace BetterAnimalsTab
{
    [StaticConstructorOnStartup]
    public static class Resources
    {
        #region Fields

        public static readonly Texture2D BarBg = SolidColorMaterials.NewSolidColorTexture( 0f, 1f, 0f, 0.8f );

        public static readonly Texture2D CheckWhite = ContentFinder<Texture2D>.Get( "UI/Buttons/CheckOnWhite" );

        public static readonly Texture2D FilterOffTex = ContentFinder<Texture2D>.Get( "UI/Buttons/filter_off_large" );

        public static readonly Texture2D FilterTex = ContentFinder<Texture2D>.Get( "UI/Buttons/filter_large" );

        public static readonly Texture2D[] GenderTextures =
        {
            ContentFinder<Texture2D>.Get( "UI/Gender/none" ),
            ContentFinder<Texture2D>.Get( "UI/Gender/male" ),
            ContentFinder<Texture2D>.Get( "UI/Gender/female" )
        };

        public static readonly Texture2D[] LifeStageTextures =
        {
            ContentFinder<Texture2D>.Get( "UI/LifeStage/1" ),
            ContentFinder<Texture2D>.Get( "UI/LifeStage/2" ),
            ContentFinder<Texture2D>.Get( "UI/LifeStage/3" ),
            ContentFinder<Texture2D>.Get( "UI/LifeStage/unknown" )
        };

        public static readonly Texture2D PregnantTex = ContentFinder<Texture2D>.Get( "UI/FilterStates/pregnant_large" );

        public static readonly Texture2D SlaughterTex = ContentFinder<Texture2D>.Get( "UI/Buttons/slaughter" );

        public static readonly Texture2D[] TrainingTextures =
        {
            ContentFinder<Texture2D>.Get( "UI/Training/obedience" ),
            ContentFinder<Texture2D>.Get( "UI/Training/release" ),
            ContentFinder<Texture2D>.Get( "UI/Training/rescue" ),
            ContentFinder<Texture2D>.Get( "UI/Training/haul" )
        };

        public static readonly Texture2D WorkBoxCheckTex = ContentFinder<Texture2D>.Get( "UI/Widgets/WorkBoxCheck" );

        #endregion Fields
    }
}