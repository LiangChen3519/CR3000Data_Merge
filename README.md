Here are C# programs for merge all data file in CR3000 data logger. The aims are:

- Get strat datetime and end datetime corss all files.
- Check all variables and remove duplicated one cross all files.
- Based on statrt datetme and end datetime, produce a full timeseries with 30 mins resolution.
- Based on the full time series, again mer all variables into one file and save as a new file.

Why we need to these full times series data file, because it helps for further processes, for example intergrated with
ERA5-LAND data series to gap-fill the missing part. In the future, the gapfill part will be also considered.
