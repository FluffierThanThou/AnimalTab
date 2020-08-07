﻿// MainTabWindow_Animals.cs
// Copyright Karel Kroeze, 2017-2017

using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using static AnimalTab.Constants;

namespace AnimalTab
{
    public class MainTabWindow_Animals: RimWorld.MainTabWindow_Animals
    {
        private static MainTabWindow_Animals _instance;

        public MainTabWindow_Animals()
        {
            _instance = this;
        }

        public static MainTabWindow_Animals Instance => _instance;
        protected override PawnTableDef PawnTableDef => PawnTableDefOf.Animals;
        protected override float ExtraTopSpace => Constants.ExtraTopSpace;

        protected override float ExtraBottomSpace
        {
            get
            {
                if ( Filter )
                    return Constants.ExtraBottomSpace + Constants.ExtraFilterSpace;
                return Constants.ExtraBottomSpace;
            }
        } 

        public override void DoWindowContents( Rect rect )
        {
            rect = DoExtraDrawers( rect );
            DoFilterBar( rect );
            base.DoWindowContents( rect );
        }

        private List<DefModExtension_DrawerExtra> _extraDrawers;
        public List<DefModExtension_DrawerExtra> ExtraDrawers
        {
            get
            {
                return _extraDrawers ??= MainButtonDefOf.Animals.modExtensions?.OfType<DefModExtension_DrawerExtra>()
                                                        .ToList() 
                                      ?? new List<DefModExtension_DrawerExtra>();
            }
        }

        private Rect DoExtraDrawers( Rect rect )
        {
            foreach ( var extraDrawer in ExtraDrawers )
                rect.yMin += extraDrawer.Worker.Draw( rect );

            return rect;
        }

        private static IEnumerable<FilterWorker> _filters;

        public static IEnumerable<FilterWorker> Filters
        {
            get
            {
                if ( _filters == null )
                    _filters = DefDatabase<FilterDef>.AllDefsListForReading.Select( f => f.Worker );
                return _filters;
            }
        }

        protected override IEnumerable<Pawn> Pawns => Filter ? FilteredPawns : AllPawns;

        public IEnumerable<Pawn> FilteredPawns
        {
            get
            {
                if (_filteredPawns == null)
                    RecachePawns();
                return _filteredPawns;
            }
        }
        private IEnumerable<Pawn> _filteredPawns;

        public IEnumerable<Pawn> AllPawns
        {
            get
            {
                if (_allPawns == null)
                    RecachePawns();
                return _allPawns;
            }
        }
        private IEnumerable<Pawn> _allPawns;

        // Note that we're overriding this _only_ because it's called from Notify_PawnsChanged, and unlike that method,
        // this one is virtual.
        protected override void SetInitialSizeAndPosition()
        {
            // don't give a damn about this part.
            base.SetInitialSizeAndPosition();

            // cache our pawn lists.
            RecachePawns();
        }

        public void RecachePawns()
        {
            _allPawns = base.Pawns;
            _filteredPawns = base.Pawns.Where( p => Filters.All( f => f.Allows( p ) ) );
        }

        private void DoFilterBar( Rect rect )
        {
            var barWidth = Filters.Count() * ( FilterButtonSize + Margin ) + Margin;
            Rect buttonRect = new Rect(rect.xMax - Margin - ButtonSize, rect.yMax - Margin - ButtonSize, ButtonSize, ButtonSize);
            Rect barRect = new Rect( buttonRect.xMin - Margin - barWidth, rect.yMax - Margin - ButtonSize, barWidth, ButtonSize );
            Rect countRect = new Rect( rect.xMin + Margin, barRect.yMin - Margin - ButtonSize * 2/3f, rect.width - ButtonSize - Margin * 3, ButtonSize * 2/3f );

            DrawFilterButton( buttonRect );
            if ( Filter )
            {
                DrawFilters( barRect, Filters );
                DrawCounts( countRect );
            }
        }

        private void DrawCounts( Rect rect )
        {
            Text.Anchor = TextAnchor.UpperRight;
            Text.Font = GameFont.Tiny;
            GUI.color = Color.grey;
            Widgets.Label( rect, "AnimalTab.XofYShown".Translate( Pawns.Count(), AllPawns.Count() ) );
            GUI.color = Color.white;
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.UpperLeft;
        }

        private void DrawFilters( Rect rect, IEnumerable<FilterWorker> filters )
        {
            Widgets.DrawBoxSolid( rect, new Color(0f, 0f, 0f, .2f) );
            Rect filterRect = new Rect( Margin, ( ButtonSize - FilterButtonSize ) / 2f, FilterButtonSize, FilterButtonSize );
            try
            {
                GUI.BeginGroup( rect );
                foreach ( var filter in filters )
                {
                    filter.Draw( filterRect );
                    filterRect.x += FilterButtonSize + Margin;
                }
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        private bool _filter;

        public bool Filter
        {
            get => _filter;
            set
            {
                if ( _filter == value )
                    return;

                _filter = value;
                Notify_PawnsChanged();
                Notify_ResolutionChanged();
            }
        }
        private void DrawFilterButton( Rect rect )
        {
            if ( Widgets.ButtonImage( rect, Resources.Filter, Filter ? GenUI.MouseoverColor : Color.white,
                Filter ? Color.white : GenUI.MouseoverColor ) )
                Filter = !Filter;
        }
    }
}