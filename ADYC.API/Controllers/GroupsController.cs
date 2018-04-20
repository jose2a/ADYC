using ADYC.API.ViewModels;
using ADYC.IService;
using ADYC.Model;
using ADYC.Util.Exceptions;
using ADYC.Util.RestUtils;
using AutoMapper;
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

            return Ok(groups
                .Select(g =>
                {
                    var groupDto = Mapper.Map<Group, GroupDto>(g);
                    groupDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Groups") + g.Id;

                    return groupDto;
                }));
        }

        // GET api/<controller>/5
        [Route("{id}")]
        [ResponseType(typeof(GroupDto))]
        public IHttpActionResult Get(int id)
        {
            var groups = _groupService.Get(id);

            if (groups != null)
            {
                var groupDto = Mapper.Map<Group, GroupDto>(groups);
                groupDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Groups") + groups.Id;
                return Ok(groupDto);
            }

            return NotFound();
        }

        [Route("GetByName/{name}")]
        [ResponseType(typeof(IEnumerable<GroupDto>))]
        public IHttpActionResult GetByName(string name)
        {
            var groups = _groupService.FindByName(name);

            return Ok(groups
                .Select(g => {
                    var groupDto = Mapper.Map<Grade, GroupDto>(g);
                    groupDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Groups") + g.Id;

                    return groupDto;
                }));
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

                    var groupDto = Mapper.Map<Group, GroupDto>(group);
                    groupDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Groups") + group.Id;

                    return CreatedAtRoute("DefaultApi", new { Id = group.Id }, groupDto);
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
                catch (NonexistingEntityException ne)
                {
                    ModelState.AddModelError("", ne.Message);
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
            }
            catch (ForeignKeyEntityException fke)
            {
                ModelState.AddModelError("", fke.Message);

                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
