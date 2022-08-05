using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Models
{
    public class SimulationCost
    {
        [Key]
        public int SimulationID { get; set; }
        public int HeaderSimulationID { get; set; }
        public decimal kurs { get; set; }
        public decimal volume2 { get; set; }
        public int operasiHari { get; set; }
        public int operasiMinggu { get; set; }
        public int operasiBulan { get; set; }
        public decimal m3hari { get; set; }
        public decimal m3jam { get; set; }
        public decimal mmbtu { get; set; }
        public decimal mmbtuBln { get; set; }
        public decimal mmbtuHari { get; set; }
        public decimal mmbtuJam { get; set; }
        public decimal gajiOperator { get; set; }
        public int jumlahOperator { get; set; }
        public decimal investasiPRS { get; set; }
        public int depresiasi { get; set; }
        public int depresiasiBulan { get; set; }
        public decimal invGTM { get; set; }
        public int depresiasiGTM { get; set; }
        public int depresiasiGTMBulan { get; set; }
        public decimal kebutuhanGTM { get; set; }
        public int jmlGTMbeli { get; set; }
        public int jmlGTMsewa { get; set; }
        public int jmlSewaHeadTruck { get; set; }
        public decimal sewaHeadTruck { get; set; }
        public decimal sewaBedTrailer { get; set; }
        public int jmlHeadTruck { get; set; }
        public int jmlBedTrailer { get; set; }
        public decimal biayaTransport { get; set; }
        public int jmlSupir { get; set; }
        public decimal gajiSupir { get; set; }
        public decimal jarakTempuh { get; set; }
        public decimal sewaGTM { get; set; }
        public string asalStation { get; set; }
        public string lokasiCapel { get; set; }
        public string UkuranGTM { get; set; }
        public string GTM { get; set; }
        public string PRS { get; set; }
        public decimal hargaGas { get; set; }
        public decimal hargaGasRp { get; set; }
        public decimal compressed { get; set; }
        public decimal compressedRp { get; set; }
        public decimal totalTransportasiCost { get; set; }
        public decimal totalTransportasiCostRp { get; set; }
        public decimal prsCost { get; set; }
        public decimal prsCostRp { get; set; }
        public decimal operatorCost { get; set; }
        public decimal operatorCostRp { get; set; }
        public decimal gtmCost { get; set; }
        public decimal gtmCostRp { get; set; }
        public decimal gtmSewaCost { get; set; }
        public decimal gtmSewaCostRp { get; set; }
        public decimal headTruckSewaCost { get; set; }
        public decimal headTruckSewaCostRp { get; set; }
        public decimal lainLain { get; set; }
        public decimal lainLainRp { get; set; }
        public decimal overheadCost { get; set; }
        public decimal overheadCostRp { get; set; }
        public decimal marginCost { get; set; }
        public decimal marginCostRp { get; set; }
        public decimal hargaJual { get; set; }
        public decimal hargaJualRp { get; set; }
        public decimal hargaM3 { get; set; }
        public decimal hargaBBM { get; set; }
        public decimal costBBMRitase { get; set; }
        public decimal ritaseBulan { get; set; }
        public decimal kapasitasGTM { get; set; }
        public decimal kapasitasGTMbeli { get; set; }
        public decimal kapasitasGTMsewa { get; set; }
        public decimal biayaBBMBulan { get; set; }
        public decimal biayaBBMIDRM3 { get; set; }
        public decimal biayaBBMUS { get; set; }
        public decimal gajiSopirBulan { get; set; }
        public decimal gajiSopirBulanIDRM3 { get; set; }
        public decimal gajiSopirUS { get; set; }
        public decimal biayaTransIDR { get; set; }
        public decimal biayaTransUS { get; set; }
        public decimal resultPendapatan { get; set; }
        public decimal resultHPP { get; set; }
        public decimal resulPendapatanKotor { get; set; }
        public decimal resultBiayaOperasi { get; set; }
        public decimal resultDepresiasi { get; set; }
        public decimal resultLaba { get; set; }
        public decimal minPrice { get; set; }
        public decimal maxPrice { get; set; }

        public string? CreatedBy { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public SimulationCost() { }
    }

    public class SP_HeaderSimulation
    {
        public int HeaderSimulationID { get; set; }
        public string ProjectName { get; set; }
        public string Creator { get; set; }
        public DateTime? ProjectDate { get; set; }
        public string CustomerName { get; set; }

        public string lokasiCapel { get; set; }
        public DateTime? TargetCOD { get; set; }
        public int HourVolume { get; set; }
        public int Volume { get; set; }
    }

    public class SP_DetailSimulation
    {
        public int SimulationID { get; set; }
        public int HeaderSimulationID { get; set; }
        public string Infrastructure { get; set; }
        public decimal HargaJual { get; set; }
        public int TargetCOD { get; set; }
        public string AsalStation { get; set; }
        public int FlagPrice { get; set; }

    }

    public class SP_CostSimulation
    {
        public int headerSimulationID { get; set; }
        public decimal volume2 { get; set; }
        public decimal jarak { get; set; }
        public int operasiHari { get; set; }
        public int operasiBulan { get; set; }
        public string energyName { get; set; }
        public string asalStation { get; set; }
        public string lokasiCapel { get; set; }
        public decimal minPrice { get; set; }
        public decimal maxPrice { get; set; }
    }
}
