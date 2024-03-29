﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hologram.Resources;

public class ArrowModel : BakedMesh
{
    protected override string Name => "Arrow";

    public override byte[] ModelData
    {
        get => new byte[] {
            0x00, 0x05, 0x41, 0x72, 0x72, 0x6F, 0x77, 0x00, 0x00, 0x00, 0xA3, 0x00, 0x00, 0x00, 0x00, 0x40, 0x10, 0x01, 0xCD, 0xBF, 0x0C, 0xBA, 0x73, 0x2E, 0x3D, 0x2E, 0x58, 0xBB, 0xEC, 0x3C, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0x47, 0xC5, 0xAC, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x7B, 0x14, 0xBA, 0x2E, 0x3D, 0x2E, 0x58, 0xBB, 0xEC, 0x3B, 0xC0, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x80, 0x00, 0x00, 0x2E, 0x3D, 0x2E, 0x58, 0xBB, 0xEC, 0x3C, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3D, 0xDB, 0xA3, 0x44, 0x40, 0x10, 0x01, 0xCD, 0xBF, 0x0A, 0x06, 0x31, 0x34, 0x9F, 0x2E, 0x58, 0xBB, 0x9E, 0x3B, 0xC0, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0xC3, 0xEF, 0x07, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x6C, 0x83, 0x66, 0x34, 0x9F, 0x2E, 0x58, 0xBB, 0x9E, 0x3B, 0x80, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0x57, 0x6A, 0xF9, 0x40, 0x10, 0x01, 0xCD, 0xBF, 0x02, 0x04, 0x19, 0x37, 0x81, 0x2E, 0x58, 0xBB, 0x05, 0x3B, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x0E, 0x39, 0xD6, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x54, 0xDB, 0x38, 0x37, 0x81, 0x2E, 0x58, 0xBB, 0x05, 0x3B, 0x40, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0x9C, 0x5E, 0x70, 0x40, 0x10, 0x01, 0xCD, 0xBE, 0xEA, 0x05, 0xDE, 0x39, 0x0D, 0x2E, 0x58, 0xBA, 0x27, 0x3B, 0x40, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x35, 0x04, 0xF7, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x35, 0x04, 0xF7, 0x39, 0x0D, 0x2E, 0x58, 0xBA, 0x27, 0x3B, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0xC7, 0x05, 0x21, 0x40, 0x10, 0x01, 0xCD, 0xBE, 0xC7, 0x05, 0x21, 0x3A, 0x27, 0x2E, 0x58, 0xB9, 0x0D, 0x3B, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x54, 0xDB, 0x38, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x0E, 0x39, 0xD6, 0x3A, 0x27, 0x2E, 0x58, 0xB9, 0x0D, 0x3A, 0xC0, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0xEA, 0x05, 0xDE, 0x40, 0x10, 0x01, 0xCD, 0xBE, 0x9C, 0x5E, 0x70, 0x3B, 0x05, 0x2E, 0x58, 0xB7, 0x81, 0x3A, 0xC0, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x6C, 0x83, 0x66, 0xC0, 0x10, 0x01, 0xCD, 0xBE, 0xC3, 0xEF, 0x07, 0x3B, 0x05, 0x2E, 0x58, 0xB7, 0x81, 0x3A, 0x80, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x02, 0x04, 0x19, 0x40, 0x10, 0x01, 0xCD, 0xBE, 0x57, 0x6A, 0xF9, 0x3B, 0x9E, 0x2E, 0x58, 0xB4, 0x9F, 0x3A, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x7B, 0x14, 0xBA, 0xC0, 0x10, 0x01, 0xCD, 0xBE, 0x47, 0xC5, 0xAC, 0x3B, 0x9E, 0x2E, 0x58, 0xB4, 0x9F, 0x3A, 0x40, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x0A, 0x06, 0x31, 0x40, 0x10, 0x01, 0xCD, 0xBD, 0xDB, 0xA3, 0x44, 0x3B, 0xEC, 0x2E, 0x58, 0xAE, 0x3D, 0x3A, 0x40, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x80, 0x00, 0x00, 0xC0, 0x10, 0x01, 0xCD, 0x00, 0x00, 0x00, 0x00, 0x3B, 0xEC, 0x2E, 0x58, 0xAE, 0x3D, 0x3A, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x0C, 0xBA, 0x73, 0x40, 0x10, 0x01, 0xCD, 0x00, 0x00, 0x00, 0x00, 0x3B, 0xEC, 0x2E, 0x58, 0x2E, 0x3D, 0x3A, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x7B, 0x14, 0xBA, 0xC0, 0x10, 0x01, 0xCD, 0x3E, 0x47, 0xC5, 0xAC, 0x3B, 0xEC, 0x2E, 0x58, 0x2E, 0x3D, 0x39, 0xC0, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x0A, 0x06, 0x31, 0x40, 0x10, 0x01, 0xCD, 0x3D, 0xDB, 0xA3, 0x44, 0x3B, 0x9E, 0x2E, 0x58, 0x34, 0x9F, 0x39, 0xC0, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x6C, 0x83, 0x66, 0xC0, 0x10, 0x01, 0xCD, 0x3E, 0xC3, 0xEF, 0x07, 0x3B, 0x9E, 0x2E, 0x58, 0x34, 0x9F, 0x39, 0x80, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x02, 0x04, 0x19, 0x40, 0x10, 0x01, 0xCD, 0x3E, 0x57, 0x6A, 0xF9, 0x3B, 0x05, 0x2E, 0x58, 0x37, 0x81, 0x39, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x54, 0xDB, 0x38, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x0E, 0x39, 0xD6, 0x3B, 0x05, 0x2E, 0x58, 0x37, 0x81, 0x39, 0x40, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0xEA, 0x05, 0xDE, 0x40, 0x10, 0x01, 0xCD, 0x3E, 0x9C, 0x5E, 0x70, 0x3A, 0x27, 0x2E, 0x58, 0x39, 0x0D, 0x39, 0x40, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x35, 0x04, 0xF7, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x35, 0x04, 0xF7, 0x3A, 0x27, 0x2E, 0x58, 0x39, 0x0D, 0x39, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0xC7, 0x05, 0x21, 0x40, 0x10, 0x01, 0xCD, 0x3E, 0xC7, 0x05, 0x21, 0x39, 0x0D, 0x2E, 0x58, 0x3A, 0x27, 0x39, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x0E, 0x39, 0xD6, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x54, 0xDB, 0x38, 0x39, 0x0D, 0x2E, 0x58, 0x3A, 0x27, 0x38, 0xC0, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0x57, 0x6A, 0xF9, 0x40, 0x10, 0x01, 0xCD, 0x3F, 0x02, 0x04, 0x19, 0x37, 0x81, 0x2E, 0x58, 0x3B, 0x05, 0x38, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0xC3, 0xEF, 0x07, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x6C, 0x83, 0x66, 0x37, 0x81, 0x2E, 0x58, 0x3B, 0x05, 0x38, 0x80, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3D, 0xDB, 0xA3, 0x44, 0x40, 0x10, 0x01, 0xCD, 0x3F, 0x0A, 0x06, 0x31, 0x34, 0x9F, 0x2E, 0x58, 0x3B, 0x9E, 0x38, 0x40, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0x47, 0xC5, 0xAC, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x7B, 0x14, 0xBA, 0x34, 0x9F, 0x2E, 0x58, 0x3B, 0x9E, 0x38, 0x40, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x80, 0x00, 0x00, 0x00, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x80, 0x00, 0x00, 0x2E, 0x3D, 0x2E, 0x58, 0x3B, 0xEC, 0x38, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x80, 0x00, 0x00, 0x00, 0x40, 0x10, 0x01, 0xCD, 0x3F, 0x0C, 0xBA, 0x73, 0xAE, 0x3D, 0x2E, 0x58, 0x3B, 0xEC, 0x38, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0x47, 0xC5, 0xAC, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x7B, 0x14, 0xBA, 0xAE, 0x3D, 0x2E, 0x58, 0x3B, 0xEC, 0x37, 0x80, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0xBD, 0xDB, 0xA3, 0x44, 0x40, 0x10, 0x01, 0xCD, 0x3F, 0x0A, 0x06, 0x31, 0xB4, 0x9F, 0x2E, 0x58, 0x3B, 0x9E, 0x37, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0xC3, 0xEF, 0x07, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x6C, 0x83, 0x66, 0xB4, 0x9F, 0x2E, 0x58, 0x3B, 0x9E, 0x37, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0x57, 0x6A, 0xF9, 0x40, 0x10, 0x01, 0xCD, 0x3F, 0x02, 0x04, 0x19, 0xB7, 0x81, 0x2E, 0x58, 0x3B, 0x05, 0x37, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x0E, 0x39, 0xD6, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x54, 0xDB, 0x38, 0xB7, 0x81, 0x2E, 0x58, 0x3B, 0x05, 0x36, 0x80, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0x9C, 0x5E, 0x70, 0x40, 0x10, 0x01, 0xCD, 0x3E, 0xEA, 0x05, 0xDE, 0xB9, 0x0D, 0x2E, 0x58, 0x3A, 0x27, 0x36, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x35, 0x04, 0xF7, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x35, 0x04, 0xF7, 0xB9, 0x0D, 0x2E, 0x58, 0x3A, 0x27, 0x36, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0xC7, 0x05, 0x21, 0x40, 0x10, 0x01, 0xCD, 0x3E, 0xC7, 0x05, 0x21, 0xBA, 0x27, 0x2E, 0x58, 0x39, 0x0D, 0x36, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x54, 0xDB, 0x27, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x0E, 0x39, 0xD6, 0xBA, 0x27, 0x2E, 0x58, 0x39, 0x0D, 0x35, 0x80, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0xEA, 0x05, 0xDE, 0x40, 0x10, 0x01, 0xCD, 0x3E, 0x9C, 0x5E, 0x70, 0xBB, 0x05, 0x2E, 0x58, 0x37, 0x81, 0x35, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x6C, 0x83, 0x66, 0xC0, 0x10, 0x01, 0xCD, 0x3E, 0xC3, 0xEF, 0x28, 0xBB, 0x05, 0x2E, 0x58, 0x37, 0x81, 0x35, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x02, 0x04, 0x19, 0x40, 0x10, 0x01, 0xCD, 0x3E, 0x57, 0x6A, 0xF9, 0xBB, 0x9E, 0x2E, 0x58, 0x34, 0x9F, 0x35, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x7B, 0x14, 0xBA, 0xC0, 0x10, 0x01, 0xCD, 0x3E, 0x47, 0xC5, 0xAC, 0xBB, 0x9E, 0x2E, 0x58, 0x34, 0x9F, 0x34, 0x80, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x0A, 0x06, 0x31, 0x40, 0x10, 0x01, 0xCD, 0x3D, 0xDB, 0xA3, 0x44, 0xBB, 0xEC, 0x2E, 0x58, 0x2E, 0x3D, 0x34, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x80, 0x00, 0x00, 0xC0, 0x10, 0x01, 0xCD, 0x80, 0x00, 0x00, 0x00, 0xBB, 0xEC, 0x2E, 0x58, 0x2E, 0x3D, 0x34, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x0C, 0xBA, 0x73, 0x40, 0x10, 0x01, 0xCD, 0x00, 0x00, 0x00, 0x00, 0xBB, 0xEC, 0x2E, 0x58, 0xAE, 0x3D, 0x34, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x7B, 0x14, 0xBA, 0xC0, 0x10, 0x01, 0xCD, 0xBE, 0x47, 0xC5, 0xAC, 0xBB, 0xEC, 0x2E, 0x58, 0xAE, 0x3D, 0x33, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x0A, 0x06, 0x31, 0x40, 0x10, 0x01, 0xCD, 0xBD, 0xDB, 0xA3, 0x44, 0xBB, 0x9E, 0x2E, 0x58, 0xB4, 0x9F, 0x33, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x6C, 0x83, 0x56, 0xC0, 0x10, 0x01, 0xCD, 0xBE, 0xC3, 0xEF, 0x28, 0xBB, 0x9E, 0x2E, 0x58, 0xB4, 0x9F, 0x32, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x02, 0x04, 0x19, 0x40, 0x10, 0x01, 0xCD, 0xBE, 0x57, 0x6A, 0xF9, 0xBB, 0x05, 0x2E, 0x58, 0xB7, 0x81, 0x32, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x54, 0xDB, 0x38, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x0E, 0x39, 0xD6, 0xBB, 0x05, 0x2E, 0x58, 0xB7, 0x81, 0x31, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0xC7, 0x05, 0x21, 0x40, 0x10, 0x01, 0xCD, 0xBE, 0xC7, 0x05, 0x21, 0xBA, 0x27, 0x2E, 0x58, 0xB9, 0x0D, 0x30, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x35, 0x04, 0xF7, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x35, 0x04, 0xF7, 0xBA, 0x27, 0x2E, 0x58, 0xB9, 0x0D, 0x30, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x0E, 0x39, 0xD6, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x54, 0xDB, 0x38, 0xB9, 0x0D, 0x2E, 0x58, 0xBA, 0x27, 0x2E, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0x9C, 0x5E, 0x70, 0x40, 0x10, 0x01, 0xCD, 0xBE, 0xEA, 0x05, 0xDE, 0xB7, 0x81, 0x2E, 0x58, 0xBB, 0x05, 0x2E, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0xC3, 0xEF, 0x07, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x6C, 0x83, 0x66, 0xB7, 0x81, 0x2E, 0x58, 0xBB, 0x05, 0x2C, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x2F, 0xE1, 0xDA, 0x40, 0x1A, 0xA2, 0x34, 0xBE, 0xEB, 0x0A, 0x91, 0x37, 0x4A, 0xBA, 0xDA, 0xB3, 0xCB, 0x3A, 0xC0, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x43, 0x6E, 0x0D, 0x40, 0x1A, 0xA2, 0x34, 0xBE, 0xA1, 0xE6, 0x47, 0x37, 0x4A, 0xBA, 0xDA, 0xB3, 0xCB, 0x3A, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0x57, 0x6A, 0xF9, 0x40, 0x10, 0x01, 0xCD, 0xBF, 0x02, 0x04, 0x19, 0xB4, 0x9F, 0x2E, 0x58, 0xBB, 0x9E, 0x2C, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0x47, 0xC5, 0xAC, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x7B, 0x14, 0xBA, 0xB4, 0x9F, 0x2E, 0x58, 0xBB, 0x9E, 0x28, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0xBD, 0xDB, 0xA3, 0x44, 0x40, 0x10, 0x01, 0xCD, 0xBF, 0x0A, 0x06, 0x31, 0xAE, 0x3D, 0x2E, 0x58, 0xBB, 0xEC, 0x28, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x80, 0x00, 0x00, 0xAE, 0x3D, 0x2E, 0x58, 0xBB, 0xEC, 0x00, 0x00, 0x38, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x7B, 0x14, 0xBA, 0xC0, 0x10, 0x01, 0xCD, 0xBE, 0x47, 0xC5, 0xAC, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3B, 0xE2, 0x34, 0xC0, 0xFF, 0xFF, 0xFF, 0x3E, 0x47, 0xC5, 0xAC, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x7B, 0x14, 0xBA, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3A, 0x60, 0x23, 0x7B, 0xFF, 0xFF, 0xFF, 0xBF, 0x7B, 0x14, 0xBA, 0xC0, 0x10, 0x01, 0xCD, 0x3E, 0x47, 0xC5, 0xAC, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x38, 0x1E, 0x32, 0x80, 0xFF, 0xFF, 0xFF, 0xBE, 0xEB, 0x0A, 0x91, 0x40, 0x1A, 0xA2, 0x34, 0xBF, 0x2F, 0xE1, 0xDA, 0xB8, 0xC6, 0x35, 0x71, 0xB9, 0xD1, 0x2E, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x15, 0x93, 0x4F, 0x40, 0x1A, 0xA2, 0x34, 0xBF, 0x15, 0x93, 0x4F, 0xB8, 0xC6, 0x35, 0x71, 0xB9, 0xD1, 0x30, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xB8, 0xC6, 0x35, 0x71, 0xB9, 0xD1, 0x2E, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0x25, 0x12, 0x23, 0x40, 0x1A, 0xA2, 0x34, 0x3F, 0x4F, 0x77, 0x9F, 0x30, 0xCC, 0xBA, 0xDA, 0x37, 0xE9, 0x38, 0x40, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x53, 0x88, 0x22, 0x40, 0x1A, 0xA2, 0x34, 0x80, 0x00, 0x00, 0x00, 0xB8, 0x1D, 0xBA, 0xDA, 0x2A, 0x7A, 0x34, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0x25, 0x12, 0x23, 0x40, 0x1A, 0xA2, 0x34, 0xBF, 0x4F, 0x77, 0x9F, 0x30, 0xCC, 0xBA, 0xDA, 0xB7, 0xE9, 0x3B, 0xC0, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0xA1, 0xE6, 0x47, 0x40, 0x1A, 0xA2, 0x34, 0xBF, 0x43, 0x6E, 0x0D, 0x30, 0xCC, 0xBA, 0xDA, 0xB7, 0xE9, 0x3B, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x43, 0x6E, 0x0D, 0x40, 0x1A, 0xA2, 0x34, 0x3E, 0xA1, 0xE6, 0x47, 0x37, 0x4A, 0xBA, 0xDA, 0x33, 0xCB, 0x39, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x2F, 0xE1, 0xDA, 0x40, 0x1A, 0xA2, 0x34, 0x3E, 0xEB, 0x0A, 0x91, 0x37, 0x4A, 0xBA, 0xDA, 0x33, 0xCB, 0x39, 0x40, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0xEB, 0x0A, 0x91, 0x40, 0x1A, 0xA2, 0x34, 0x3F, 0x2F, 0xE1, 0xDA, 0xB5, 0x3E, 0xBA, 0xDA, 0x36, 0x64, 0x36, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x15, 0x93, 0x4F, 0x40, 0x1A, 0xA2, 0x34, 0x3F, 0x15, 0x93, 0x4F, 0xB5, 0x3E, 0xBA, 0xDA, 0x36, 0x64, 0x36, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x4F, 0x77, 0x9F, 0x40, 0x1A, 0xA2, 0x34, 0xBE, 0x25, 0x12, 0x23, 0x37, 0xE9, 0xBA, 0xDA, 0xB0, 0xCC, 0x3A, 0x40, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x80, 0x00, 0x00, 0x00, 0x40, 0x1A, 0xA2, 0x34, 0x3F, 0x53, 0x88, 0x22, 0x2A, 0x7A, 0xBA, 0xDA, 0x38, 0x1D, 0x38, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x4F, 0x77, 0x9F, 0x40, 0x1A, 0xA2, 0x34, 0xBE, 0x25, 0x12, 0x23, 0xB8, 0x1D, 0xBA, 0xDA, 0xAA, 0x7A, 0x33, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0xEB, 0x0A, 0x91, 0x40, 0x1A, 0xA2, 0x34, 0xBF, 0x2F, 0xE1, 0xDA, 0x33, 0xCB, 0xBA, 0xDA, 0xB7, 0x4A, 0x3B, 0x40, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x15, 0x93, 0x4F, 0x40, 0x1A, 0xA2, 0x34, 0x3F, 0x15, 0x93, 0x4F, 0x36, 0x64, 0xBA, 0xDA, 0x35, 0x3E, 0x39, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x2F, 0xE1, 0xDA, 0x40, 0x1A, 0xA2, 0x34, 0x3E, 0xEB, 0x0A, 0xB3, 0xB6, 0x64, 0xBA, 0xDA, 0x35, 0x3E, 0x35, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0xA1, 0xE6, 0x47, 0x40, 0x1A, 0xA2, 0x34, 0xBF, 0x43, 0x6E, 0x0D, 0xB3, 0xCB, 0xBA, 0xDA, 0xB7, 0x4A, 0x2C, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x53, 0x88, 0x22, 0x40, 0x1A, 0xA2, 0x34, 0x00, 0x00, 0x00, 0x00, 0x38, 0x1D, 0xBA, 0xDA, 0xAA, 0x7A, 0x3A, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0x25, 0x12, 0x23, 0x40, 0x1A, 0xA2, 0x34, 0x3F, 0x4F, 0x77, 0x9F, 0xAA, 0x7A, 0xBA, 0xDA, 0x38, 0x1D, 0x37, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x43, 0x6E, 0x0D, 0x40, 0x1A, 0xA2, 0x34, 0xBE, 0xA1, 0xE6, 0x47, 0xB7, 0xE9, 0xBA, 0xDA, 0xB0, 0xCC, 0x32, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x15, 0x93, 0x4F, 0x40, 0x1A, 0xA2, 0x34, 0xBF, 0x15, 0x93, 0x4F, 0x35, 0x3E, 0xBA, 0xDA, 0xB6, 0x64, 0x3B, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0xEB, 0x0A, 0x91, 0x40, 0x1A, 0xA2, 0x34, 0x3F, 0x2F, 0xE1, 0xDA, 0x35, 0x3E, 0xBA, 0xDA, 0x36, 0x64, 0x38, 0xC0, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0x9C, 0x5E, 0x70, 0x40, 0x10, 0x01, 0xCD, 0x3E, 0xEA, 0x05, 0xDE, 0x35, 0x3E, 0xBA, 0xDA, 0x36, 0x64, 0x38, 0xC0, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x43, 0x6E, 0x0D, 0x40, 0x1A, 0xA2, 0x34, 0x3E, 0xA1, 0xE6, 0x47, 0xB7, 0x4A, 0xBA, 0xDA, 0x33, 0xCB, 0x35, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0x25, 0x12, 0x23, 0x40, 0x1A, 0xA2, 0x34, 0xBF, 0x4F, 0x77, 0x9F, 0xB0, 0xCC, 0xBA, 0xDA, 0xB7, 0xE9, 0x28, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x4F, 0x77, 0x9F, 0x40, 0x1A, 0xA2, 0x34, 0x3E, 0x25, 0x12, 0x23, 0x38, 0x1D, 0xBA, 0xDA, 0x2A, 0x7A, 0x39, 0xC0, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0xA1, 0xE6, 0x47, 0x40, 0x1A, 0xA2, 0x34, 0x3F, 0x43, 0x6E, 0x0D, 0xB0, 0xCC, 0xBA, 0xDA, 0x37, 0xE9, 0x37, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x2F, 0xE1, 0xDA, 0x40, 0x1A, 0xA2, 0x34, 0xBE, 0xEB, 0x0A, 0x91, 0xB7, 0x4A, 0xBA, 0xDA, 0xB3, 0xCB, 0x31, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0xEA, 0x05, 0xDE, 0x40, 0x10, 0x01, 0xCD, 0xBE, 0x9C, 0x5E, 0x70, 0xB7, 0x4A, 0xBA, 0xDA, 0xB3, 0xCB, 0x31, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x3E, 0xA1, 0xE6, 0x47, 0x40, 0x1A, 0xA2, 0x34, 0x3F, 0x43, 0x6E, 0x0D, 0x33, 0xCB, 0xBA, 0xDA, 0x37, 0x4A, 0x38, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x4F, 0x77, 0x9F, 0x40, 0x1A, 0xA2, 0x34, 0x3E, 0x25, 0x12, 0x23, 0xB7, 0xE9, 0xBA, 0xDA, 0x30, 0xCC, 0x34, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x1A, 0xA2, 0x34, 0xBF, 0x53, 0x88, 0x22, 0x2A, 0x7A, 0xBA, 0xDA, 0xB8, 0x1D, 0x3C, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x1A, 0xA2, 0x34, 0xBF, 0x53, 0x88, 0x22, 0xAA, 0x7A, 0xBA, 0xDA, 0xB8, 0x1D, 0x00, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x10, 0x01, 0xCD, 0xBF, 0x0C, 0xBA, 0x73, 0xAA, 0x7A, 0xBA, 0xDA, 0xB8, 0x1D, 0x00, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x2D, 0xE7, 0x35, 0x71, 0x3B, 0x7D, 0x38, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x37, 0x18, 0x35, 0x71, 0xBA, 0xA3, 0x3B, 0x40, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xB7, 0x18, 0x35, 0x71, 0xBA, 0xA3, 0x2C, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xAD, 0xE7, 0x35, 0x71, 0x3B, 0x7D, 0x37, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x38, 0xC6, 0x35, 0x71, 0xB9, 0xD1, 0x3B, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xB4, 0x5E, 0x35, 0x71, 0xBB, 0x33, 0x28, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xB4, 0x5E, 0x35, 0x71, 0x3B, 0x33, 0x37, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x39, 0xD1, 0x35, 0x71, 0xB8, 0xC6, 0x3A, 0xC0, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xAD, 0xE7, 0x35, 0x71, 0xBB, 0x7D, 0x00, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xB7, 0x18, 0x35, 0x71, 0x3A, 0xA3, 0x36, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x3A, 0xA3, 0x35, 0x71, 0xB7, 0x18, 0x3A, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xB8, 0xC6, 0x35, 0x71, 0x39, 0xD1, 0x36, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x3B, 0x33, 0x35, 0x71, 0xB4, 0x5E, 0x3A, 0x40, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xB9, 0xD1, 0x35, 0x71, 0x38, 0xC6, 0x35, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x3B, 0x7D, 0x35, 0x71, 0xAD, 0xE7, 0x3A, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xBA, 0xA3, 0x35, 0x71, 0x37, 0x18, 0x35, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x3B, 0x7D, 0x35, 0x71, 0x2D, 0xE7, 0x39, 0xC0, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xBB, 0x33, 0x35, 0x71, 0x34, 0x5E, 0x34, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x3B, 0x33, 0x35, 0x71, 0x34, 0x5E, 0x39, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xBB, 0x7D, 0x35, 0x71, 0x2D, 0xE7, 0x34, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x3A, 0xA3, 0x35, 0x71, 0x37, 0x18, 0x39, 0x40, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xBB, 0x7D, 0x35, 0x71, 0xAD, 0xE7, 0x33, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x39, 0xD1, 0x35, 0x71, 0x38, 0xC6, 0x39, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xBB, 0x33, 0x35, 0x71, 0xB4, 0x5E, 0x32, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x38, 0xC6, 0x35, 0x71, 0x39, 0xD1, 0x38, 0xC0, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xBA, 0xA3, 0x35, 0x71, 0xB7, 0x18, 0x31, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x37, 0x18, 0x35, 0x71, 0x3A, 0xA3, 0x38, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x2D, 0xE7, 0x35, 0x71, 0xBB, 0x7D, 0x3B, 0xC0, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0xB9, 0xD1, 0x35, 0x71, 0xB8, 0xC6, 0x30, 0x00, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x34, 0x5E, 0x35, 0x71, 0x3B, 0x33, 0x38, 0x40, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x40, 0x96, 0x16, 0x61, 0x00, 0x00, 0x00, 0x00, 0x34, 0x5E, 0x35, 0x71, 0xBB, 0x33, 0x3B, 0x80, 0x3C, 0x00, 0xFF, 0xFF, 0xFF, 0xBE, 0x47, 0xC5, 0xAC, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x7B, 0x14, 0xBA, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x39, 0xA0, 0x37, 0xC4, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x80, 0x00, 0x00, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3A, 0x00, 0x37, 0xD7, 0xFF, 0xFF, 0xFF, 0x3E, 0x47, 0xC5, 0xAC, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x7B, 0x14, 0xBA, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3A, 0x60, 0x37, 0xC4, 0xFF, 0xFF, 0xFF, 0x3E, 0xC3, 0xEF, 0x07, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x6C, 0x83, 0x66, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3A, 0xBC, 0x37, 0x8C, 0xFF, 0xFF, 0xFF, 0x3F, 0x0E, 0x39, 0xD6, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x54, 0xDB, 0x38, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3B, 0x11, 0x37, 0x31, 0xFF, 0xFF, 0xFF, 0x3F, 0x35, 0x04, 0xF7, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x35, 0x04, 0xF7, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3B, 0x5C, 0x36, 0xB7, 0xFF, 0xFF, 0xFF, 0x3F, 0x54, 0xDB, 0x38, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x0E, 0x39, 0xD6, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3B, 0x99, 0x36, 0x22, 0xFF, 0xFF, 0xFF, 0x3F, 0x6C, 0x83, 0x66, 0xC0, 0x10, 0x01, 0xCD, 0xBE, 0xC3, 0xEF, 0x07, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3B, 0xC6, 0x35, 0x78, 0xFF, 0xFF, 0xFF, 0x3F, 0x80, 0x00, 0x00, 0xC0, 0x10, 0x01, 0xCD, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3B, 0xEC, 0x34, 0x00, 0xFF, 0xFF, 0xFF, 0x3F, 0x7B, 0x14, 0xBA, 0xC0, 0x10, 0x01, 0xCD, 0x3E, 0x47, 0xC5, 0xAC, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3B, 0xE2, 0x32, 0x80, 0xFF, 0xFF, 0xFF, 0x3F, 0x6C, 0x83, 0x66, 0xC0, 0x10, 0x01, 0xCD, 0x3E, 0xC3, 0xEF, 0x07, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3B, 0xC6, 0x31, 0x10, 0xFF, 0xFF, 0xFF, 0x3F, 0x54, 0xDB, 0x38, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x0E, 0x39, 0xD6, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3B, 0x99, 0x2F, 0x77, 0xFF, 0xFF, 0xFF, 0x3F, 0x35, 0x04, 0xF7, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x35, 0x04, 0xF7, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3B, 0x5C, 0x2D, 0x24, 0xFF, 0xFF, 0xFF, 0x3F, 0x0E, 0x39, 0xD6, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x54, 0xDB, 0x38, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3B, 0x11, 0x2A, 0x75, 0xFF, 0xFF, 0xFF, 0x3E, 0xC3, 0xEF, 0x07, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x6C, 0x83, 0x66, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3A, 0xBC, 0x27, 0x3D, 0xFF, 0xFF, 0xFF, 0x80, 0x00, 0x00, 0x00, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x80, 0x00, 0x00, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x3A, 0x00, 0x21, 0x1F, 0xFF, 0xFF, 0xFF, 0xBE, 0x47, 0xC5, 0xAC, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x7B, 0x14, 0xBA, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x39, 0xA0, 0x23, 0x7B, 0xFF, 0xFF, 0xFF, 0xBE, 0xC3, 0xEF, 0x07, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x6C, 0x83, 0x66, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x39, 0x44, 0x27, 0x3D, 0xFF, 0xFF, 0xFF, 0xBF, 0x0E, 0x39, 0xD6, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x54, 0xDB, 0x38, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x38, 0xEF, 0x2A, 0x75, 0xFF, 0xFF, 0xFF, 0xBF, 0x35, 0x04, 0xF7, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x35, 0x04, 0xF7, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x38, 0xA4, 0x2D, 0x24, 0xFF, 0xFF, 0xFF, 0xBF, 0x54, 0xDB, 0x27, 0xC0, 0x10, 0x01, 0xCD, 0x3F, 0x0E, 0x39, 0xD6, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x38, 0x67, 0x2F, 0x77, 0xFF, 0xFF, 0xFF, 0xBF, 0x6C, 0x83, 0x66, 0xC0, 0x10, 0x01, 0xCD, 0x3E, 0xC3, 0xEF, 0x28, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x38, 0x3A, 0x31, 0x10, 0xFF, 0xFF, 0xFF, 0xBF, 0x80, 0x00, 0x00, 0xC0, 0x10, 0x01, 0xCD, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x38, 0x14, 0x34, 0x00, 0xFF, 0xFF, 0xFF, 0xBF, 0x7B, 0x14, 0xBA, 0xC0, 0x10, 0x01, 0xCD, 0xBE, 0x47, 0xC5, 0xAC, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x38, 0x1E, 0x34, 0xC0, 0xFF, 0xFF, 0xFF, 0xBF, 0x6C, 0x83, 0x56, 0xC0, 0x10, 0x01, 0xCD, 0xBE, 0xC3, 0xEF, 0x28, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x38, 0x3A, 0x35, 0x78, 0xFF, 0xFF, 0xFF, 0xBF, 0x54, 0xDB, 0x38, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x0E, 0x39, 0xD6, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x38, 0x67, 0x36, 0x22, 0xFF, 0xFF, 0xFF, 0xBF, 0x35, 0x04, 0xF7, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x35, 0x04, 0xF7, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x38, 0xA4, 0x36, 0xB7, 0xFF, 0xFF, 0xFF, 0xBF, 0x0E, 0x39, 0xD6, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x54, 0xDB, 0x38, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x38, 0xEF, 0x37, 0x31, 0xFF, 0xFF, 0xFF, 0xBE, 0xC3, 0xEF, 0x07, 0xC0, 0x10, 0x01, 0xCD, 0xBF, 0x6C, 0x83, 0x66, 0x00, 0x00, 0xBC, 0x00, 0x00, 0x00, 0x39, 0x44, 0x37, 0x8C, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x02, 0x3A, 0x00, 0x00, 0x00, 0x01, 0x00, 0x02, 0x00, 0x03, 0x00, 0x04, 0x00, 0x01, 0x00, 0x05, 0x00, 0x06, 0x00, 0x04, 0x00, 0x07, 0x00, 0x08, 0x00, 0x06, 0x00, 0x09, 0x00, 0x0A, 0x00, 0x08, 0x00, 0x0B, 0x00, 0x0C, 0x00, 0x0A, 0x00, 0x0D, 0x00, 0x0E, 0x00, 0x0C, 0x00, 0x0F, 0x00, 0x10, 0x00, 0x0E, 0x00, 0x11, 0x00, 0x12, 0x00, 0x10, 0x00, 0x13, 0x00, 0x14, 0x00, 0x12, 0x00, 0x15, 0x00, 0x16, 0x00, 0x14, 0x00, 0x17, 0x00, 0x18, 0x00, 0x16, 0x00, 0x19, 0x00, 0x1A, 0x00, 0x18, 0x00, 0x1A, 0x00, 0x1B, 0x00, 0x1C, 0x00, 0x1C, 0x00, 0x1D, 0x00, 0x1E, 0x00, 0x1D, 0x00, 0x1F, 0x00, 0x1E, 0x00, 0x20, 0x00, 0x21, 0x00, 0x1F, 0x00, 0x22, 0x00, 0x23, 0x00, 0x21, 0x00, 0x24, 0x00, 0x25, 0x00, 0x23, 0x00, 0x26, 0x00, 0x27, 0x00, 0x25, 0x00, 0x28, 0x00, 0x29, 0x00, 0x27, 0x00, 0x2A, 0x00, 0x2B, 0x00, 0x29, 0x00, 0x2C, 0x00, 0x2D, 0x00, 0x2B, 0x00, 0x2E, 0x00, 0x2F, 0x00, 0x2D, 0x00, 0x30, 0x00, 0x31, 0x00, 0x2F, 0x00, 0x32, 0x00, 0x33, 0x00, 0x31, 0x00, 0x34, 0x00, 0x35, 0x00, 0x33, 0x00, 0x35, 0x00, 0x36, 0x00, 0x37, 0x00, 0x36, 0x00, 0x38, 0x00, 0x37, 0x00, 0x39, 0x00, 0x3A, 0x00, 0x38, 0x00, 0x0D, 0x00, 0x3B, 0x00, 0x3C, 0x00, 0x3D, 0x00, 0x3E, 0x00, 0x3A, 0x00, 0x3F, 0x00, 0x40, 0x00, 0x3E, 0x00, 0x41, 0x00, 0x42, 0x00, 0x43, 0x00, 0x44, 0x00, 0x45, 0x00, 0x46, 0x00, 0x1B, 0x00, 0x47, 0x00, 0x1D, 0x00, 0x2E, 0x00, 0x48, 0x00, 0x30, 0x00, 0x05, 0x00, 0x49, 0x00, 0x4A, 0x00, 0x17, 0x00, 0x4B, 0x00, 0x4C, 0x00, 0x28, 0x00, 0x4D, 0x00, 0x4E, 0x00, 0x39, 0x00, 0x45, 0x00, 0x44, 0x00, 0x0D, 0x00, 0x4F, 0x00, 0x0F, 0x00, 0x20, 0x00, 0x47, 0x00, 0x50, 0x00, 0x30, 0x00, 0x51, 0x00, 0x32, 0x00, 0x05, 0x00, 0x52, 0x00, 0x07, 0x00, 0x17, 0x00, 0x53, 0x00, 0x19, 0x00, 0x28, 0x00, 0x54, 0x00, 0x2A, 0x00, 0x39, 0x00, 0x55, 0x00, 0x3D, 0x00, 0x0F, 0x00, 0x56, 0x00, 0x11, 0x00, 0x20, 0x00, 0x57, 0x00, 0x22, 0x00, 0x32, 0x00, 0x58, 0x00, 0x34, 0x00, 0x07, 0x00, 0x59, 0x00, 0x09, 0x00, 0x19, 0x00, 0x5A, 0x00, 0x5B, 0x00, 0x2C, 0x00, 0x54, 0x00, 0x5C, 0x00, 0x3D, 0x00, 0x5D, 0x00, 0x3F, 0x00, 0x11, 0x00, 0x5E, 0x00, 0x13, 0x00, 0x22, 0x00, 0x5F, 0x00, 0x24, 0x00, 0x34, 0x00, 0x60, 0x00, 0x61, 0x00, 0x0B, 0x00, 0x59, 0x00, 0x3B, 0x00, 0x5B, 0x00, 0x62, 0x00, 0x1B, 0x00, 0x2C, 0x00, 0x63, 0x00, 0x2E, 0x00, 0x03, 0x00, 0x64, 0x00, 0x49, 0x00, 0x3F, 0x00, 0x65, 0x00, 0x66, 0x00, 0x13, 0x00, 0x4B, 0x00, 0x15, 0x00, 0x26, 0x00, 0x5F, 0x00, 0x4D, 0x00, 0x36, 0x00, 0x60, 0x00, 0x45, 0x00, 0x50, 0x00, 0x47, 0x00, 0x67, 0x00, 0x52, 0x00, 0x4A, 0x00, 0x68, 0x00, 0x55, 0x00, 0x44, 0x00, 0x69, 0x00, 0x57, 0x00, 0x50, 0x00, 0x6A, 0x00, 0x59, 0x00, 0x52, 0x00, 0x6B, 0x00, 0x5D, 0x00, 0x55, 0x00, 0x6C, 0x00, 0x5F, 0x00, 0x57, 0x00, 0x6D, 0x00, 0x3B, 0x00, 0x59, 0x00, 0x6E, 0x00, 0x65, 0x00, 0x5D, 0x00, 0x6F, 0x00, 0x4D, 0x00, 0x5F, 0x00, 0x70, 0x00, 0x3C, 0x00, 0x3B, 0x00, 0x71, 0x00, 0x4E, 0x00, 0x4D, 0x00, 0x72, 0x00, 0x4F, 0x00, 0x3C, 0x00, 0x73, 0x00, 0x54, 0x00, 0x4E, 0x00, 0x74, 0x00, 0x56, 0x00, 0x4F, 0x00, 0x75, 0x00, 0x5C, 0x00, 0x54, 0x00, 0x76, 0x00, 0x5E, 0x00, 0x56, 0x00, 0x77, 0x00, 0x63, 0x00, 0x5C, 0x00, 0x78, 0x00, 0x4B, 0x00, 0x5E, 0x00, 0x79, 0x00, 0x48, 0x00, 0x63, 0x00, 0x7A, 0x00, 0x4C, 0x00, 0x4B, 0x00, 0x7B, 0x00, 0x51, 0x00, 0x48, 0x00, 0x7C, 0x00, 0x53, 0x00, 0x4C, 0x00, 0x7D, 0x00, 0x58, 0x00, 0x51, 0x00, 0x7E, 0x00, 0x5A, 0x00, 0x53, 0x00, 0x7F, 0x00, 0x60, 0x00, 0x58, 0x00, 0x80, 0x00, 0x62, 0x00, 0x5A, 0x00, 0x81, 0x00, 0x49, 0x00, 0x64, 0x00, 0x82, 0x00, 0x45, 0x00, 0x60, 0x00, 0x83, 0x00, 0x47, 0x00, 0x62, 0x00, 0x84, 0x00, 0x4A, 0x00, 0x49, 0x00, 0x85, 0x00, 0x00, 0x00, 0x03, 0x00, 0x01, 0x00, 0x03, 0x00, 0x05, 0x00, 0x04, 0x00, 0x05, 0x00, 0x07, 0x00, 0x06, 0x00, 0x07, 0x00, 0x09, 0x00, 0x08, 0x00, 0x09, 0x00, 0x0B, 0x00, 0x0A, 0x00, 0x0B, 0x00, 0x0D, 0x00, 0x0C, 0x00, 0x0D, 0x00, 0x0F, 0x00, 0x0E, 0x00, 0x0F, 0x00, 0x11, 0x00, 0x10, 0x00, 0x11, 0x00, 0x13, 0x00, 0x12, 0x00, 0x13, 0x00, 0x15, 0x00, 0x14, 0x00, 0x15, 0x00, 0x17, 0x00, 0x16, 0x00, 0x17, 0x00, 0x19, 0x00, 0x18, 0x00, 0x19, 0x00, 0x5B, 0x00, 0x1A, 0x00, 0x1A, 0x00, 0x5B, 0x00, 0x1B, 0x00, 0x1C, 0x00, 0x1B, 0x00, 0x1D, 0x00, 0x1D, 0x00, 0x20, 0x00, 0x1F, 0x00, 0x20, 0x00, 0x22, 0x00, 0x21, 0x00, 0x22, 0x00, 0x24, 0x00, 0x23, 0x00, 0x24, 0x00, 0x26, 0x00, 0x25, 0x00, 0x26, 0x00, 0x28, 0x00, 0x27, 0x00, 0x28, 0x00, 0x2A, 0x00, 0x29, 0x00, 0x2A, 0x00, 0x2C, 0x00, 0x2B, 0x00, 0x2C, 0x00, 0x2E, 0x00, 0x2D, 0x00, 0x2E, 0x00, 0x30, 0x00, 0x2F, 0x00, 0x30, 0x00, 0x32, 0x00, 0x31, 0x00, 0x32, 0x00, 0x34, 0x00, 0x33, 0x00, 0x34, 0x00, 0x61, 0x00, 0x35, 0x00, 0x35, 0x00, 0x61, 0x00, 0x36, 0x00, 0x36, 0x00, 0x39, 0x00, 0x38, 0x00, 0x39, 0x00, 0x3D, 0x00, 0x3A, 0x00, 0x0D, 0x00, 0x0B, 0x00, 0x3B, 0x00, 0x3D, 0x00, 0x3F, 0x00, 0x3E, 0x00, 0x3F, 0x00, 0x66, 0x00, 0x40, 0x00, 0x86, 0x00, 0x87, 0x00, 0x88, 0x00, 0x88, 0x00, 0x89, 0x00, 0x8A, 0x00, 0x8A, 0x00, 0x8B, 0x00, 0x8C, 0x00, 0x8C, 0x00, 0x8D, 0x00, 0x41, 0x00, 0x41, 0x00, 0x8E, 0x00, 0x8F, 0x00, 0x8F, 0x00, 0x90, 0x00, 0x91, 0x00, 0x91, 0x00, 0x92, 0x00, 0x93, 0x00, 0x93, 0x00, 0x94, 0x00, 0x42, 0x00, 0x42, 0x00, 0x95, 0x00, 0x96, 0x00, 0x96, 0x00, 0x97, 0x00, 0x98, 0x00, 0x98, 0x00, 0x99, 0x00, 0x9A, 0x00, 0x9A, 0x00, 0x9B, 0x00, 0x43, 0x00, 0x43, 0x00, 0x9C, 0x00, 0x9D, 0x00, 0x9D, 0x00, 0x9E, 0x00, 0x43, 0x00, 0x9E, 0x00, 0x9F, 0x00, 0x43, 0x00, 0x9F, 0x00, 0xA0, 0x00, 0xA1, 0x00, 0xA1, 0x00, 0xA2, 0x00, 0x9F, 0x00, 0xA2, 0x00, 0x86, 0x00, 0x9F, 0x00, 0x86, 0x00, 0x88, 0x00, 0x41, 0x00, 0x88, 0x00, 0x8A, 0x00, 0x41, 0x00, 0x8A, 0x00, 0x8C, 0x00, 0x41, 0x00, 0x41, 0x00, 0x8F, 0x00, 0x91, 0x00, 0x91, 0x00, 0x93, 0x00, 0x41, 0x00, 0x93, 0x00, 0x42, 0x00, 0x41, 0x00, 0x42, 0x00, 0x96, 0x00, 0x43, 0x00, 0x96, 0x00, 0x98, 0x00, 0x43, 0x00, 0x98, 0x00, 0x9A, 0x00, 0x43, 0x00, 0x43, 0x00, 0x9F, 0x00, 0x86, 0x00, 0x86, 0x00, 0x41, 0x00, 0x43, 0x00, 0x1B, 0x00, 0x62, 0x00, 0x47, 0x00, 0x2E, 0x00, 0x63, 0x00, 0x48, 0x00, 0x05, 0x00, 0x03, 0x00, 0x49, 0x00, 0x17, 0x00, 0x15, 0x00, 0x4B, 0x00, 0x28, 0x00, 0x26, 0x00, 0x4D, 0x00, 0x39, 0x00, 0x36, 0x00, 0x45, 0x00, 0x0D, 0x00, 0x3C, 0x00, 0x4F, 0x00, 0x20, 0x00, 0x1D, 0x00, 0x47, 0x00, 0x30, 0x00, 0x48, 0x00, 0x51, 0x00, 0x05, 0x00, 0x4A, 0x00, 0x52, 0x00, 0x17, 0x00, 0x4C, 0x00, 0x53, 0x00, 0x28, 0x00, 0x4E, 0x00, 0x54, 0x00, 0x39, 0x00, 0x44, 0x00, 0x55, 0x00, 0x0F, 0x00, 0x4F, 0x00, 0x56, 0x00, 0x20, 0x00, 0x50, 0x00, 0x57, 0x00, 0x32, 0x00, 0x51, 0x00, 0x58, 0x00, 0x07, 0x00, 0x52, 0x00, 0x59, 0x00, 0x19, 0x00, 0x53, 0x00, 0x5A, 0x00, 0x2C, 0x00, 0x2A, 0x00, 0x54, 0x00, 0x3D, 0x00, 0x55, 0x00, 0x5D, 0x00, 0x11, 0x00, 0x56, 0x00, 0x5E, 0x00, 0x22, 0x00, 0x57, 0x00, 0x5F, 0x00, 0x34, 0x00, 0x58, 0x00, 0x60, 0x00, 0x0B, 0x00, 0x09, 0x00, 0x59, 0x00, 0x5B, 0x00, 0x5A, 0x00, 0x62, 0x00, 0x2C, 0x00, 0x5C, 0x00, 0x63, 0x00, 0x03, 0x00, 0x00, 0x00, 0x64, 0x00, 0x3F, 0x00, 0x5D, 0x00, 0x65, 0x00, 0x13, 0x00, 0x5E, 0x00, 0x4B, 0x00, 0x26, 0x00, 0x24, 0x00, 0x5F, 0x00, 0x36, 0x00, 0x61, 0x00, 0x60
    };
    }
}
