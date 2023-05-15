
using uint8_t = System.Byte;
using uint16_t = System.UInt16;
using uint32_t = System.UInt32;
using uint64_t = System.UInt64;

using int8_t = System.SByte;
using int16_t = System.Int16;
using int32_t = System.Int32;
using int64_t = System.Int64;

using float32 = System.Single;

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace DroneCAN
{
    public partial class DroneCAN {
        static void encode_uavcan_protocol_param_GetSet_req(uavcan_protocol_param_GetSet_req msg, dronecan_serializer_chunk_cb_ptr_t chunk_cb, object ctx, bool fdcan) {
            uint8_t[] buffer = new uint8_t[8];
            _encode_uavcan_protocol_param_GetSet_req(buffer, msg, chunk_cb, ctx, !fdcan);
        }

        static uint32_t decode_uavcan_protocol_param_GetSet_req(CanardRxTransfer transfer, uavcan_protocol_param_GetSet_req msg, bool fdcan) {
            uint32_t bit_ofs = 0;
            _decode_uavcan_protocol_param_GetSet_req(transfer, ref bit_ofs, msg, !fdcan);
            return (bit_ofs+7)/8;
        }

        static void _encode_uavcan_protocol_param_GetSet_req(uint8_t[] buffer, uavcan_protocol_param_GetSet_req msg, dronecan_serializer_chunk_cb_ptr_t chunk_cb, object ctx, bool tao) {
            memset(buffer,0,8);
            canardEncodeScalar(buffer, 0, 13, msg.index);
            chunk_cb(buffer, 13, ctx);
            _encode_uavcan_protocol_param_Value(buffer, msg.value, chunk_cb, ctx, false);
            if (!tao) {
                memset(buffer,0,8);
                canardEncodeScalar(buffer, 0, 7, msg.name_len);
                chunk_cb(buffer, 7, ctx);
            }
            for (int i=0; i < msg.name_len; i++) {
                    memset(buffer,0,8);
                    canardEncodeScalar(buffer, 0, 8, msg.name[i]);
                    chunk_cb(buffer, 8, ctx);
            }
        }

        static void _decode_uavcan_protocol_param_GetSet_req(CanardRxTransfer transfer,ref uint32_t bit_ofs, uavcan_protocol_param_GetSet_req msg, bool tao) {

            canardDecodeScalar(transfer, bit_ofs, 13, false, ref msg.index);
            bit_ofs += 13;

            _decode_uavcan_protocol_param_Value(transfer, ref bit_ofs, msg.value, false);

            if (!tao) {
                canardDecodeScalar(transfer, bit_ofs, 7, false, ref msg.name_len);
                bit_ofs += 7;
            } else {
                msg.name_len = (uint8_t)(((transfer.payload_len*8)-bit_ofs)/8);
            }

            msg.name = new uint8_t[msg.name_len];
            for (int i=0; i < msg.name_len; i++) {
                canardDecodeScalar(transfer, bit_ofs, 8, false, ref msg.name[i]);
                bit_ofs += 8;
            }

        }
    }
}