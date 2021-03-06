// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace ConnegWebSite
{
    [DisplayColumn("Name")]
    public class User
    {
        [Required]
        [MinLength(4)]
        public string Name { get; set; }
        public string Address { get; set; }
    }
}