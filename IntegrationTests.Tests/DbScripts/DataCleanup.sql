﻿EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT all'
EXEC sp_msforeachtable 'DELETE ?'
EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all'