using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.Mappers
{
    public static class FungsiUserMappers
    {
        
         public static FungsiUserDto MapToDto(this FungsiUser? dto)
        {
            if (dto == null) return new FungsiUserDto();

            return new FungsiUserDto
            {
                fungsi_id = dto.fungsi_id,
                nama = dto.nama,
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }
    }
}