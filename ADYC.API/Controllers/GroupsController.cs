﻿using ADYC.API.ViewModels;
using ADYC.IService;
using ADYC.Model;
using ADYC.Util.Exceptions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace ADYC.API.Controllers
{
    [RoutePrefix("api/Groups")]
    public class GroupsController : ADYCBasedApiController
    {
        private IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        // GET api/Groups
        [Route("")]
        [ResponseType(typeof(IEnumerable<GroupDto>))]
        public IHttpActionResult Get()
        {
            var groups = _groupService.GetAll();

            return base.Ok(groups
                .Select(g =>
                {
                    return GetGroupDto(g);
                }));
        }

        // GET api/Groups/5
        [Route("{id}")]
        [ResponseType(typeof(GroupDto))]
        public IHttpActionResult Get(int id)
        {
            var group = _groupService.Get(id);

            if (group != null)
            {
                return Ok(GetGroupDto(group));
            }

            return NotFound();
        }

        // GET api/Groups/GetByName/A
        [Route("GetByName/{name}")]
        [ResponseType(typeof(IEnumerable<GroupDto>))]
        public IHttpActionResult GetByName(string name)
        {
            try
            {
                var groups = _groupService.FindByName(name);

                return Ok(groups
                    .Select(g =>
                    {
                        return GetGroupDto(g);
                    }));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        // POST api/Groups
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(GroupDto))]
        public IHttpActionResult Post([FromBody] GroupDto form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var group = Mapper.Map<GroupDto, Group>(form);

                    _groupService.Add(group);

                    var groupDto = GetGroupDto(group);

                    return Created(new Uri(groupDto.Url), groupDto);
                }
                catch (ArgumentNullException ane)
                {
                    ModelState.AddModelError("", ane.Message);
                }
                catch (PreexistingEntityException pe)
                {
                    ModelState.AddModelError("", pe.Message);
                }
            }

            return BadRequest(ModelState);
        }

        // PUT api/Groups/5
        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(int id, [FromBody] GroupDto form)
        {
            if (ModelState.IsValid)
            {
                var groupInDb = _groupService.Get(id);

                if (groupInDb == null)
                {
                    return BadRequest();
                }

                try
                {
                    Mapper.Map(form, groupInDb);

                    _groupService.Update(groupInDb);

                    return Ok();
                }
                catch (ArgumentNullException ane)
                {
                    ModelState.AddModelError("", ane.Message);
                }
            }

            return BadRequest(ModelState);
        }

        // DELETE api/Groups/5
        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(int id)
        {
            var groupInDb = _groupService.Get(id);

            if (groupInDb == null)
            {
                return NotFound();
            }

            try
            {
                _groupService.Remove(groupInDb);

                return Ok();
            }
            catch (ForeignKeyEntityException fke)
            {
                ModelState.AddModelError("", fke.Message);               
            }

            return BadRequest(ModelState);
        }
    }
}
