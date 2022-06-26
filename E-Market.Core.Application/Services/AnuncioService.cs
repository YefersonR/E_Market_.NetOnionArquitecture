﻿using E_Market.Core.Application.Interfaces;
using E_Market.Core.Application.Interfaces.Services;
using E_Market.Core.Application.ViewModels;
using E_Market.Core.Application.ViewModels.Anuncio;
using E_Market.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Market.Core.Application.Services
{
    public class AnuncioService : IAnuncioService
    {
        public readonly IAnuncioRepository _anuncioRepository;
        public AnuncioService(IAnuncioRepository anuncioRepository)
        {
            _anuncioRepository = anuncioRepository;
        }

        public async Task Add(SaveAnuncioViewModel vm)
        {
            Anuncio anuncio = new();
            anuncio.Nombre = vm.Nombre;
            anuncio.Precio = vm.Precio;
            anuncio.Imagen = vm.Imagen;
            anuncio.Descripcion = vm.Descripcion;
            anuncio.CategoriaId = vm.CategoriaId;

            await _anuncioRepository.AddAsync(anuncio);
        }

        public async Task Update(SaveAnuncioViewModel vm)
        {
            Anuncio anuncio = new();
            anuncio.Id = vm.Id;
            anuncio.Nombre = vm.Nombre;
            anuncio.Precio = vm.Precio;
            anuncio.Imagen = vm.Imagen;
            anuncio.Descripcion = vm.Descripcion;
            anuncio.CategoriaId = vm.CategoriaId;
            
            await _anuncioRepository.UpdateAsync(anuncio);
        }

        public async Task Delete(int id)
        {
            var anuncio = await _anuncioRepository.GetByIdAsync(id);
            await _anuncioRepository.DeleteAsync(anuncio);
        }

        public async Task<SaveAnuncioViewModel> GetByIdSaveViewModel(int id)
        {
            var anuncio = await _anuncioRepository.GetByIdAsync(id);
            SaveAnuncioViewModel vm = new();
            vm.Id = anuncio.Id;
            vm.Nombre = anuncio.Nombre;
            vm.Descripcion = anuncio.Descripcion;
            vm.Precio = anuncio.Precio;
            vm.Imagen = anuncio.Imagen;
            vm.CategoriaId = anuncio.CategoriaId;
            
            return vm;
        }
        public async Task<List<AnuncioViewModel>> GetAllViewModel()
        {
            var list = await _anuncioRepository.GetAllWithIncludeAsync(new List<string> { "Categoria" });
            return list.Select(anuncio => new AnuncioViewModel
            {
                Id = anuncio.Id,
                Nombre = anuncio.Nombre,
                Descripcion = anuncio.Descripcion,
                Precio = anuncio.Precio,
                Imagen = anuncio.Imagen,
                Categoria = anuncio.Categoria.Nombre,
                CategoriaId = anuncio.Categoria.Id,
                Created = anuncio.Created



            }).ToList();
        }

        public async Task<List<AnuncioViewModel>> GetAllViewModelWithFilter(FilterAnuncioViewModel filter)
        {
            var list = await _anuncioRepository.GetAllWithIncludeAsync(new List<string> { "Categoria" });
            var listVM = list.Select(anuncio => new AnuncioViewModel
            {
                Id = anuncio.Id,
                Nombre = anuncio.Nombre,
                Descripcion = anuncio.Descripcion,
                Precio = anuncio.Precio,
                Imagen = anuncio.Imagen,
                Categoria = anuncio.Categoria.Nombre,
                CategoriaId = anuncio.Categoria.Id

            })  .ToList();
            if (filter.CategoriaId != null)
            {
                listVM = listVM.Where(anuncio => anuncio.CategoriaId == filter.CategoriaId.Value).ToList();
            }

            return listVM;
            
        }

    }
}