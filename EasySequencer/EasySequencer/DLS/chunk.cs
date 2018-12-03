using System;

namespace DLS {
	unsafe public class Chunk : IDisposable {
		protected CK_CHUNK* mp_chunk;
		protected CK_LIST* mp_list;

		protected Chunk() { }

		public virtual void Dispose() { }

		protected void Load(byte* ptr, uint size) {
			var ptrTerm = ptr + size;

			while (ptr < ptrTerm) {
				mp_chunk = (CK_CHUNK*)ptr;
				ptr += sizeof(CK_CHUNK);

				if (CHUNK_TYPE.LIST == mp_chunk->type) {
					mp_list = (CK_LIST*)ptr;
					LoadList(ptr + sizeof(CK_LIST), mp_chunk->size);
				}
				else {
					LoadChunk(ptr);
				}

				ptr += mp_chunk->size;
			}
		}

		protected virtual void LoadChunk(byte* ptr) { }

		protected virtual void LoadList(byte* ptr, uint size) { }
	}
}
