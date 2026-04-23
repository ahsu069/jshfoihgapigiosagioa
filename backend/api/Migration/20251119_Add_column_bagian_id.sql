ALTER TABLE [transaksi_history]
ALTER COLUMN approval_manajemen_pekerja_id UNIQUEIDENTIFIER NULL;

ALTER TABLE [transaksi_history]
ALTER COLUMN approval_gudang_id UNIQUEIDENTIFIER NULL;

ALTER TABLE [transaksi_history]
ALTER COLUMN approval_sectionhead_id UNIQUEIDENTIFIER NULL;

ALTER TABLE [usersCache]
ADD
    bagian_pekerja NVARCHAR(120) NULL;
		
ALTER TABLE [pekerjaTemp]
ADD
    bagian_id int NULL;
    
UPDATE uc
SET uc.bagian_pekerja = COALESCE(bu.nama, '')
FROM [usersCache] uc
LEFT JOIN SIGAPUser su ON su.user_id = uc.user_id
LEFT JOIN BagianUser bu ON bu.bagian_id = su.bagian_id;