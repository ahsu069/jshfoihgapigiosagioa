namespace api.Models.Mappers
{
    public static class SigapUserMappers
    {
        public static SigapUserDto MapToDto(this SigapUser? dto)
        {
            if (dto == null) return new SigapUserDto();

            return new SigapUserDto
            {
                user_id = dto.user_id,
                nama = dto.nama,
                bagian_id = dto.bagian_id,
                BagianUserDto = dto.BagianUserDto.MapToDto(),
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss"),
                username = dto.username,
                password = dto.password,
                UserRoleDto = dto.UserRoleDto.MapToDto(),
            };
        }
        public static SigapUser MapToDtoFromCreate(this SigapUserRequest? dto)
        {
            if (dto == null) return new SigapUser();

            return new SigapUser
            {
                nama = dto.nama,
                bagian_id = dto.bagian_id,
                username = dto.username,
            };
        }
        public static ProfileDto ProfileMapToDto(this SigapUser? dto, FungsiUser fungsiUser, Role role, List<PermissionDto> permissions)
        {
            if (dto == null) return new ProfileDto();

            return new ProfileDto
            {
                user_id = dto.user_id,
                nama = dto.nama,
                bagian_id = dto.bagian_id,
                BagianUserDto = dto.BagianUserDto.MapToDto(),
                FungsiUserDto = fungsiUser.MapToDto(),
                RoleDto = role.MapToDto(),
                PermissionDto = permissions,
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }
        public static UsersCacheDto UsersCacheMapToDto(this UsersCache dto)
        {
            if (dto == null) return new UsersCacheDto();

            return new UsersCacheDto
            {
                user_id = dto.user_id,
                nama_pekerja = dto.nama_pekerja,
                fungsi_pekerja = dto.fungsi_pekerja,
                bagian_pekerja = dto.bagian_pekerja,
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }
        public static EmployeeDto EmployeeMapToDto(this Employee dto)
        {
            if (dto == null) return new EmployeeDto();

            return new EmployeeDto
            {
                pekerja_temp_id = dto.pekerja_temp_id,
                nama_pekerja = dto.nama_pekerja,
                fungsi_pekerja = dto.fungsi_pekerja,
                id_finger = dto.id_finger,
                perusahaan_pekerja = dto.perusahaan_pekerja,
                link_file_pendukung = dto.link_file_pendukung,
                bagian_id = dto.bagian_id,
                BagianUserDto = dto.BagianUserDto.MapToDto(),
                created_at = dto.created_at.ToString("dd/MM/yyyy HH:mm:ss"),
                updated_at = dto.updated_at.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }
    }
}