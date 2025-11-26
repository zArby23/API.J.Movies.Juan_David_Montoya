using API.J.Movies.DAL.Dtos.Movie;
using API.J.Movies.DAL.Models;
using API.J.Movies.Repository.IRepository;
using API.J.Movies.Services.IServices;
using AutoMapper;
using System.Collections;

namespace API.J.Movies.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieService(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }
        public Task<bool> MovieExistsByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MovieExistsByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<MovieDto> CreateMovieAsync(MovieCreateUpdateDto movieCreateDto)
        {
            var movieExists = await _movieRepository.MovieExistsByNameAsync(movieCreateDto.Name);
            if (movieExists)
            {
                throw new InvalidOperationException($"Ya existe una película con el nombre '{movieCreateDto.Name}'");
            }

            var movie = _mapper.Map<Movie>(movieCreateDto);

            var movieCreated = await _movieRepository.CreateMovieAsync(movie);

            if (!movieCreated)
            {
                throw new Exception("Ocurrió un error al crear la película.");
            }

            return _mapper.Map<MovieDto>(movie);
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            await GetMovieByIdAsync(id);
            var isDeleted = await _movieRepository.DeleteMovieAsync(id);

            if (!isDeleted)
            {
                throw new Exception("Ocurrió un error al eliminar la película.");
            }
            return isDeleted;
        }

        public async Task<ICollection<MovieDto>> GetMoviesAsync()
        {
            
            var movies = await _movieRepository.GetMoviesAsync();
            
            return _mapper.Map<ICollection<MovieDto>>(movies);
        }

        public async Task<MovieDto> GetMovieAsync(int id)
        {
            
            var movie = await GetMovieByIdAsync(id);

            return _mapper.Map<MovieDto>(movie);
        }

        public async Task<MovieDto> UpdateMovieAsync(MovieCreateUpdateDto dto, int id)
        {
            var existingMovie = await GetMovieByIdAsync(id);

            var nameExists = await _movieRepository.MovieExistsByNameAsync(dto.Name);

            if (nameExists)
            {
                throw new InvalidOperationException($"Ya existe una película con el nombre '{dto.Name}'");
            }

            _mapper.Map(dto, existingMovie);

            var isUpdated = await _movieRepository.UpdateMovieAsync(existingMovie);

            if (!isUpdated)
            {
                throw new Exception("Ocurrió un error al actualizar la película.");
            }

            return _mapper.Map<MovieDto>(existingMovie);
        }

        private async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movie = await _movieRepository.GetMovieAsync(id);
            if (movie == null)
            {
                throw new InvalidOperationException($"No se encontró una película con el ID {id}.");
            }
            return movie;
        }
    }
}
