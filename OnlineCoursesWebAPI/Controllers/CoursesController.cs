using Data;
using AutoMapper;
using Core.Models;
using Core.Services;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OnlineCoursesWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController(
    IMapper mapper,
    IFilesService filesService,
    OnlineCoursesDbContext context
    ) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var courses = context.Courses
            .Include(x => x.Category)
            .Include(x => x.Level)
            .ToList();
        var result = mapper.Map<IEnumerable<CourseModel>>(courses);

        return Ok(result); // 200
    }

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        var course = context.Courses.Find(id);
        if (course is null) return NotFound(); // 404

        context.Entry(course).Reference(x => x.Category).Load();
        context.Entry(course).Reference(x => x.Level).Load();

        return Ok(mapper.Map<CourseModel>(course));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CourseModelCreate model)
    {
        if (!ModelState.IsValid) return BadRequest();

        var entity = mapper.Map<Course>(model);

        // save file to server
        if (model.Image is not null)
            entity.ImageUrl = await filesService.SaveImage(model.Image);

        context.Courses.Add(entity);
        context.SaveChanges();

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Edit([FromForm] CourseModelEdit model)
    {
        if (!ModelState.IsValid) return BadRequest();

        var entity = mapper.Map<Course>(model);

        // rewrite file on server
        if (model.Image is not null)
            entity.ImageUrl = await filesService.EditImage(model.Image, entity.ImageUrl);

        context.Courses.Update(entity);
        context.SaveChanges();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var entity = context.Courses.Find(id);
        if (entity is null) return NotFound();

        // delete file
        if (entity.ImageUrl is not null)
            await filesService.DeleteImage(entity.ImageUrl);

        context.Courses.Remove(entity);
        context.SaveChanges();

        return Ok();
    }
}
