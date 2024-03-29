﻿using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using BaseCore.DataBase;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Server.Models;

namespace Server.Controllers
{
    public class ControllerNTimeBase : ApiController
    {
        protected ContextProvider ContextProvider = new ContextProvider();
        protected ApplicationDbContext ApplicationDbContext = new ApplicationDbContext();
        protected CompetitionRepository CompetitionRepository;
        protected Competition Competition;

        protected ControllerNTimeBase()
        {
            CompetitionRepository = new CompetitionRepository(ContextProvider);
        }

        protected virtual async Task<bool> InitCompetitionByRelatedEntitieId<T>(int id)
            where T: class, ICompetitionId, IEntityId
        {
            Competition = await CompetitionRepository.GetByRelatedEntityId<T>(id);
            if (Competition == null)
                return false;
            return true;
        }

        protected virtual async Task<bool> InitCompetitionById(int Id)
        {
            Competition = await CompetitionRepository.GetById(Id);
            if (Competition == null)
                return false;
            return true;
        }

        protected async Task<bool> CanOrganizerAccess()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var s = UserManager.GetRoles(User.Identity.GetUserId());
            if (s[0] == "Organizer" &&
                !await CompetitionRepository.CanOrganizerEdit(User.Identity.GetUserId(), Competition))
                return false;
            return true;
        }

        protected async Task<bool> CanOrganizerAccessAndEdit()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var s = UserManager.GetRoles(User.Identity.GetUserId());
            if (s[0] == "Organizer" &&
                (!await CompetitionRepository.CanOrganizerEdit(User.Identity.GetUserId(), Competition) ||
                Competition.OrganizerEditLock))
                return false;
            return true;
        }

        protected bool CanPlayerAccess(PlayerAccount playerAccount)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var s = UserManager.GetRoles(User.Identity.GetUserId());
            if (s[0] == "Player" &&
                playerAccount.AccountId != User.Identity.GetUserId())
                return false;
            return true;
        }

        protected bool AmI(string who)
        {
            if (User.Identity.IsAuthenticated == false)
                return false;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var s = UserManager.GetRoles(User.Identity.GetUserId());
            if (s[0] != who)
                return false;
            return true;
        }

        public static string PasswordGenerator()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray()) + "a0";
        }
    }
}