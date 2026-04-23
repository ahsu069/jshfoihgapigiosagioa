-- delete transaction
DELETE FROM transaksi_detail;
DELETE FROM transaksi_history;
DELETE FROM approval_status;

-- approval_role_map migration script
DELETE FROM approval_role_map;
INSERT INTO approval_role_map (approval_role_map_id, legacy_code, role_id, note, created_at, updated_at)
SELECT NEWID() as approval_role_map_id, '1' as legacy_code, r.role_id, CONCAT('Legacy 1 -> ', r.name ) as note, GETDATE() as created_at, GETDATE() as updated_at FROM [role] r
WHERE r.code = 'SH_NON_SAFETY'
UNION 
SELECT NEWID() as approval_role_map_id, '2' as legacy_code, r.role_id, CONCAT('Legacy 2 -> ', r.name ) as note, GETDATE() as created_at, GETDATE() as updated_at FROM [role] r
WHERE r.code = 'SH_SAFETY'
UNION 
SELECT NEWID() as approval_role_map_id, '3' as legacy_code, r.role_id, CONCAT('Legacy 3 -> ', r.name ) as note, GETDATE() as created_at, GETDATE() as updated_at FROM [role] r
WHERE r.code = 'ADMIN_GUDANG';

-- kategori_transaksi migration script
DELETE FROM kategori_transaksi;
INSERT INTO kategori_transaksi (kategori_transact_id, nama_kategori_transact, created_at, updated_at)
VALUES ('IN', 'IN', GETDATE(), GETDATE()), ('OUT', 'OUT', GETDATE(), GETDATE());