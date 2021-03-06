﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.TestModels.SpatialModel;
using NetTopologySuite;

namespace Microsoft.EntityFrameworkCore
{
    public abstract class SpatialFixtureBase : SharedStoreFixtureBase<SpatialContext>
    {
        private readonly IGeometryFactory _geometryFactory
            = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 0);

        protected override string StoreName
            => "SpatialTest";

        protected override void OnModelCreating(ModelBuilder modelBuilder, DbContext context)
        {
            modelBuilder.Entity<PointEntity>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<LineStringEntity>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<PolygonEntity>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<MultiLineStringEntity>().Property(e => e.Id).ValueGeneratedNever();

            modelBuilder.Entity<GeoPointEntity>(
                b =>
                {
                    b.Property(e => e.Id).ValueGeneratedNever();
                    b.Property(e => e.Location).HasConversion(
                        v => _geometryFactory.CreatePoint(new Coordinate(v.Lat, v.Lon)),
                        v => new GeoPoint(v.X, v.Y));
                });
        }

        protected override void Seed(SpatialContext context)
            => SpatialContext.Seed(context, _geometryFactory);
    }
}
