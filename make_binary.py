#!/usr/bin/env python
# -*- coding: utf-8 -*-

import os
import sys
import struct

#==============================================================================
#
def func():

  path = 'testbin.dat'

  with open(path, 'wb') as f:
    fmt = 'b'
    b = struct.pack(fmt, 32)
    f.write(b)

    fmt = 'b'
    b = struct.pack(fmt, 32)
    f.write(b)

    fmt = 'h'
    b = struct.pack(fmt, 456)
    f.write(b)

    fmt = 'i'
    b = struct.pack(fmt, 689)
    f.write(b)

    fmt = 'i'
    b = struct.pack(fmt, 789)
    f.write(b)

    fmt = 'f'
    b = struct.pack(fmt, 32.26)
    f.write(b)

    fmt = 'd'
    b = struct.pack(fmt, 958.25432)
    f.write(b)

    b = bytes('abcdefg', 'ansi')
    f.write(b)

    b = bytes('あいうえお', 'utf8')
    f.write(b)

    b = bytes('かきくけこ', 'utf-16')
    f.write(b)


#==============================================================================

def main():

  func()


#==============================================================================

if __name__ == '__main__':
  main()
