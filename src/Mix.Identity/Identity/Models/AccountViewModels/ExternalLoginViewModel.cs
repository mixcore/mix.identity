// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;

namespace Mix.Identity.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}