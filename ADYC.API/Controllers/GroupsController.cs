using ADYC.API.ViewModels;
using ADYC.IService;
using ADYC.Model;
using ADYC.Util.Exceptions;
using ADYC.Util.RestUtils;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace ADYC.API.Controllers
{
    [RoutePrefix("api/Groups")]
    public class GroupsController : ApiController
    {
        private IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        // GET api/<controller>
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

        // GET api/<controller>/5
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

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(GroupDto))]
        // POST api/<controller>
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

        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        // PUT api/<controller>/5
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

        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        // DELETE api/<controller>/5
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

        private GroupDto GetGroupDto(Group g)
        {
            var groupDto = Mapper.Map<Group, GroupDto>(g);
            groupDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Groups") + g.Id;
            return groupDto;
        }
    }
}
